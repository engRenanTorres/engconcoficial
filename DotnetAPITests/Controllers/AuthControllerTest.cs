
using DotnetAPI.Controllers;
using DotnetAPI.Helpers;
using DotnetAPI.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAPITests.src.Controllers
{
    public class AuthControllerTest
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly AuthHelper _authHelper;
        private readonly AuthController _authController;

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
            _authController = new AuthController(_logger,_userService,configuration,_authService);
            
        }

        [Fact]
        public void NetworkService_SendPing_ReturnString()
        {
            //A.CallTo(() => _dNS.SendDNS()).Returns(false);
            //Act
            var result = "oi";

            //Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("oi");
            result.Should().Contain("o", Exactly.Once());
        }


    }
}
