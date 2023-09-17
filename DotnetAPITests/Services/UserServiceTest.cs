
using DotnetAPI.Controllers;
using DotnetAPI.Data.Repositories;
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

namespace DotnetAPITests.Services
{
    public class UserServiceTest
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<IUserService> _logger;
        private readonly UserService _userService;
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

        public UserServiceTest()
        {
            _userRepository = A.Fake<IUserRepository>();
            _logger = A.Fake<ILogger<UserService>>();
            _userService = new UserService(_logger, _userRepository);
        }


    }
}
