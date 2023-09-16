
using DotnetAPI.Controllers;
using DotnetAPI.DTOs;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAPITests.Controllers
{
    public class AuthControllerTest
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly AuthHelper _authHelper;
        private readonly AuthController _authController;
        private readonly User _user = new()
        {
            Id = 1,
            Name = "Test User",
            Email = "x@x.com",
            Password = new byte[] { Convert.ToByte('p') },
            Role = DotnetAPI.Enums.Roles.User,
            Website = null,
            SocialMedia = null,
        };

        public AuthControllerTest()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"Key1", "Value1"},
                {"Nested:Key1", "NestedValue1"},
                {"Nested:Key2", "NestedValue2"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            _userService = A.Fake<IUserService>();
            _authService = A.Fake<IAuthService>();
            _logger = A.Fake<ILogger<AuthController>>();
            _authHelper = A.Fake<AuthHelper>();
            _authController = new AuthController(_logger, _userService, configuration, _authService);

        }

        [Fact]
        public async Task Register_ReturnCreatedActionResult()
        {
            CreateUserDTO createUserDTO = new()
            {
                Name = "Test User",
                Email = "x@x.com",
                Password = "p",
            };


            A.CallTo(() => _authService.Register(createUserDTO)).Returns(_user);

            var result = await _authController.Register(createUserDTO);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status201Created);

            createdResult?.Value.Should().BeSameAs(_user);
        }

        [Fact]
        public async Task Register_InvalidUser_ThrowsException()
        {
            var createUserDTO = new CreateUserDTO();

            A.CallTo(() => _authService.Register(A<CreateUserDTO>._)).Returns(Task.FromResult<User?>(null));

            // Act and Assert
            await _authController
                .Invoking(async c => await c.Register(createUserDTO))
                .Should()
                .ThrowAsync<Exception>()
                .WithMessage("Error to Add this User");
        }

        [Fact]
        public async Task Login_ReturnOk()
        {
            CreateUserDTO createUserDTO = new()
            {
                Name = "Test User",
                Email = "x@x.com",
                Password = "p",
            };

            var loginDTO = new LoginDTO() { Email = "Test@test.com", Password = "x" };
            var fakeToken = "fake-token";

            A.CallTo(() => _authService.Login(loginDTO)).Returns(fakeToken);


            var actionResult = await _authController.Login(loginDTO);

            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okResult = actionResult.Result as OkObjectResult;

            okResult?.StatusCode.Should().Be(200);

            okResult?.Value.Should().BeOfType<Dictionary<string, string>>();
            var tokenData = 
                okResult?.Value as Dictionary<string, string> ?? 
                new Dictionary<string, string> { { "", "" } };

            tokenData.Should().ContainKey("token"); 
            tokenData["token"].Should().Be(fakeToken);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ThrowsBadRequest()
        {
            var loginDTO = new LoginDTO() { Email = "Test@test.com", Password = "x" };

            A.CallTo(() => _authService.Login(A<LoginDTO>._)).Returns(Task.FromResult<string?>(null));
            var actionResult = await _authController.Login(loginDTO);
            
            actionResult.Result.Should().NotBeNull(); 
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>(); 
            var badRequestResult = actionResult.Result as BadRequestObjectResult;

            badRequestResult?.StatusCode.Should().Be(400);

            badRequestResult?.Value.Should().Be("Invalid email or password.");
        }

        [Fact]
        public async Task RefreshToken_ReturnOk()
        {
            CreateUserDTO createUserDTO = new()
            {
                Name = "Test User",
                Email = "x@x.com",
                Password = "p",
            };

            var httpContext = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", "1") 
            }));
            httpContext.User = userClaims;
            _authController.ControllerContext.HttpContext = httpContext;

            var authUserId = "1"; 
            var fakeToken = "fake-token";

            A.CallTo(() => _userService.GetUser(1)).Returns(Task.FromResult(_user)); 
            A.CallTo(() => _authService.RefreshToken(authUserId, _user.Role)).Returns(fakeToken); 

            var result = await _authController.RefreshToken();


            result.Should().BeOfType<OkObjectResult>(); 
            var okResult = (OkObjectResult)result;

            okResult.StatusCode.Should().Be(200); 

            okResult.Value.Should().BeOfType<Dictionary<string, string>>(); 
            var tokenData = okResult.Value 
                as Dictionary<string, string> ??
                new Dictionary<string, string> { { "", "" } };

            tokenData.Should().ContainKey("token"); 
            tokenData["token"].Should().Be(fakeToken);
        }

        [Fact]
        public async Task RefreshToken_InvalidUserId_ReturnsBadRequest()
        { 
            var httpContext = new DefaultHttpContext();
            _authController.ControllerContext.HttpContext = httpContext;
            // Act
            var result = await _authController.RefreshToken();

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = (BadRequestObjectResult)result;

            badRequestResult.StatusCode.Should().Be(400); 

            badRequestResult.Value.Should().Be("UserId cannot be converted into int type");
        }

        [Fact]
        public async Task RefreshToken_UserNotFound_ReturnsBadRequest()
        {
            var httpContext = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", "1") 
            }));
            httpContext.User = userClaims;
            _authController.ControllerContext.HttpContext = httpContext;

            var authUserId = "1";

            A.CallTo(() => _userService.GetUser(1)).Returns(Task.FromResult<User?>(null)); 

            var result = await _authController.RefreshToken();

            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = (BadRequestObjectResult)result;

            badRequestResult.StatusCode.Should().Be(400); 

            badRequestResult.Value.Should().Be("User does not exist"); 
        }


        [Fact]
        public async Task RefreshToken_TokenRefreshFailed_ReturnsNotFound()
        {
            var httpContext = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", "1") 
            }));
            httpContext.User = userClaims;
            _authController.ControllerContext.HttpContext = httpContext;

            var authUserId = "1"; 

            A.CallTo(() => _userService.GetUser(1)).Returns(Task.FromResult(_user));
            A.CallTo(() => _authService.RefreshToken(authUserId, _user.Role)).Returns<string?>(null); 

            var result = await _authController.RefreshToken();

            result.Should().BeOfType<NotFoundResult>(); 
        }

    }
}
