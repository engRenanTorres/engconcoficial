
using DotnetAPI.Controllers;
using DotnetAPI.DTOs;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        public async Task GetUsers_ReturnUsers()
        {
            var users = new List<User>();
            users.Add(_user);
            A.CallTo(() => _userService.GetAllUsers()).Returns(users);

            var result = await _userController.GetUsers();

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(users);
        }

        [Fact]
        public async Task GetUser_ReturnUser()
        {
            A.CallTo(() => _userService.GetUser(1)).Returns(Task.FromResult<User?>(_user));

            var result = await _userController.GetUser(1);

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(_user);
        }

        [Fact]
        public async Task GetUser_NotFoundUser_ThrowsNotFound()
        {
            var createUserDTO = new CreateUserDTO();

            A.CallTo(() => _userService.GetUser(1)).Returns(Task.FromResult<User?>(null));

            var actionResult = await _userController.GetUser(1);

            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var badRequestResult = actionResult.Result as NotFoundObjectResult;

            badRequestResult?.StatusCode.Should().Be(404);

            badRequestResult?.Value.Should().Be("User id: 1 not found");
        }

    }
}
