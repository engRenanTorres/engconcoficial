﻿
using DotnetAPI.Controllers;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DotnetAPITests.Controllers
{
    public class UserControllerTest
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly UserController _userController;
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

        public UserControllerTest()
        {
            _userService = A.Fake<IUserService>();
            _logger = A.Fake<ILogger<UserController>>();
            _userController = new UserController(_userService, _logger);
        }

        [Fact]
        public async Task GetUser_ReturnOkUser()
        {
            A.CallTo(() => _userService.GetUser(1)).Returns(Task.FromResult<User?>(_user));

            var result = await _userController.GetUserById(1);

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(_user);
        }

        [Fact]
        public async Task GetUser_NotFoundUser_ThrowsNotFound()
        {
            A.CallTo(() => _userService.GetUser(1)).Returns(Task.FromResult<User?>(null));

            var actionResult = await _userController.GetUserById(1);

            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var badRequestResult = actionResult.Result as NotFoundObjectResult;

            badRequestResult?.StatusCode.Should().Be(404);

            badRequestResult?.Value.Should().Be("User id: 1 not found");
        }
        [Fact]
        public async Task GetUserByEmail_ReturnOkUser()
        {
            A.CallTo(() => _userService.GetUserByEmail("email@gmail.com")).Returns(Task.FromResult<User?>(_user));

            var result = await _userController.GetUserByEmail("email@gmail.com");

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(_user);
        }

        [Fact]
        public async Task GetUserByEmail_NotFoundUser_ThrowsNotFound()
        {
            A.CallTo(() => _userService.GetUserByEmail("email@gmail.com")).Returns(Task.FromResult<User?>(null));

            var actionResult = await _userController.GetUserByEmail("email@gmail.com");

            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var badRequestResult = actionResult.Result as NotFoundObjectResult;

            badRequestResult?.StatusCode.Should().Be(404);

            badRequestResult?.Value.Should().Be("User email: email@gmail.com not found");
        }

        [Fact]
        public async Task GetUsers_ReturnOkUsers()
        {
            var users = new List<User>
            {
                _user
            };
            A.CallTo(() => _userService.GetAllUsers()).Returns(users);

            var result = await _userController.GetUsers();

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(users);
        }

        [Fact]
        public async Task PatchUser_ReturnOkUser()
        {
            var newUserData = new UpdateUserDTO()
            {
                Name = "Test",
            };
            var httpContext = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", "1")
            }));
            httpContext.User = userClaims;
            _userController.ControllerContext.HttpContext = httpContext;

            var authUserId = "1";

            A.CallTo(() => _userService.PatchUser(authUserId, newUserData)).Returns(Task.FromResult<User?>(_user));

            var result = await _userController.PatchUser(newUserData);

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(_user);
        }

        [Fact]
        public async Task PatchUser_InvalidUserId_ThrowsNotFound()
        {
            var updateUserDTO = new UpdateUserDTO();
            A.CallTo(() => _userService.PatchUser("1", updateUserDTO)).Returns(Task.FromResult<User?>(null));

            var actionResult = await _userController.PatchUser(updateUserDTO);

            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var badRequestResult = actionResult.Result as NotFoundObjectResult;

            badRequestResult?.StatusCode.Should().Be(404);

            badRequestResult?.Value.Should().Be("User id: 0 not found");
        }

        [Fact]
        public async Task PatchUser_NotFoundUser_ThrowsNotFound()
        {
            var updateUserDTO = new UpdateUserDTO();
            var httpContext = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", "1")
            }));
            httpContext.User = userClaims;
            _userController.ControllerContext.HttpContext = httpContext;

            A.CallTo(() => _userService.PatchUser("1", updateUserDTO)).Returns(Task.FromResult<User?>(null));

            var actionResult = await _userController.PatchUser(updateUserDTO);

            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var badRequestResult = actionResult.Result as NotFoundObjectResult;

            badRequestResult?.StatusCode.Should().Be(404);

            badRequestResult?.Value.Should().Be("User id: 1 not found");
        }

        [Fact]
        public async Task DeleteUser_ReturnNoContent()
        {
            var authUserId = 1;

            A.CallTo(() => _userService.DeleteUser(authUserId)).Returns(Task.FromResult<bool?>(true));

            var result = await _userController.DeleteUser(authUserId);


            var createdResult = result as NoContentResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task DeleteUser_InvalidUserId_ThrowsNotFound()
        {
            var authUserId = 1;
            A.CallTo(() => _userService.DeleteUser(authUserId)).Returns(Task.FromResult<bool?>(null));

            var result = await _userController.DeleteUser(authUserId);

            var actionResult = result as NotFoundObjectResult;

            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<NotFoundObjectResult>();

            actionResult?.StatusCode.Should().Be(404);
        }

    }
}
