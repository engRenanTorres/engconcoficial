using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using DotnetAPI.Services;
using Microsoft.Extensions.Logging;

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

        [Fact]
        public async Task GetUser_ReturnUser()
        {
            A.CallTo(() => _userRepository.GetSingleUser(1)).Returns(Task.FromResult<User?>(_user));

            var result = await _userService.GetUser(1);

            result.Should().BeOfType<User>();
            result?.Should().BeSameAs(_user);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnUser()
        {
            var email = "x@x.com";
            A.CallTo(() => _userRepository.GetSingleUserByEmail(email)).Returns(Task.FromResult<User?>(_user));

            var result = await _userService.GetUserByEmail(email);

            result.Should().BeOfType<User>();
            result?.Should().BeSameAs(_user);
        }

        [Fact]
        public async Task GetAllUsers_ReturnUsers()
        {
            var users = new List<User>
            {
                _user
            };
            A.CallTo(() => _userRepository.GetAllUsers()).Returns(users);

            var result = await _userService.GetAllUsers();

            result.Should().BeOfType<List<User>>();

            result?.Should().BeSameAs(users);
        }

        [Fact]
        public async Task DeleteUser_ReturnTrue()
        {
            var authUserId = 1;

            A.CallTo(() => _userRepository.GetSingleUser(authUserId)).Returns(Task.FromResult<User?>(_user));
            A.CallTo(() => _userRepository.RemoveEntity(_user));
            A.CallTo(() => _userRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

            var result = await _userService.DeleteUser(authUserId);

            result?.Should().Be(true);
        }

        [Fact]
        public async Task DeleteUser_NotFoundUser_ReturnNull()
        {
            var authUserId = 1;

            A.CallTo(() => _userRepository.GetSingleUser(authUserId)).Returns(Task.FromResult<User?>(null));

            var result = await _userService.DeleteUser(authUserId);

            result.Should().Be(null);
        }

        [Fact]
        public async Task DeleteUser_UnnableToDelete_ReturnFalse()
        {
            var authUserId = 1;

            A.CallTo(() => _userRepository.GetSingleUser(authUserId)).Returns(Task.FromResult<User?>(null));
            A.CallTo(() => _userRepository.RemoveEntity(_user));
            A.CallTo(() => _userRepository.SaveChanges()).Returns(Task.FromResult<bool>(false));

            var result = await _userService.DeleteUser(authUserId);

            result?.Should().Be(false);
        }

        [Fact]
        public async Task PatchUser_updateSucessfully_ReturnUser()
        {
            var updateUserDTO = new UpdateUserDTO();
            var authUserId = "1";

            A.CallTo(() => _userRepository.GetSingleUser(int.Parse(authUserId))).Returns(Task.FromResult<User?>(_user));
            A.CallTo(() => _userRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));


            var result = await _userService.PatchUser(authUserId, updateUserDTO);

            result.Should().Be(_user);
        }


        [Fact]
        public async Task PatchUser_NotFoundUser_ReturnNull()
        {
            var updateUserDTO = new UpdateUserDTO();
            var authUserId = "1";

            A.CallTo(() => _userRepository.GetSingleUser(int.Parse(authUserId))).Returns(Task.FromResult<User?>(null));

            var result = await _userService.PatchUser(authUserId, updateUserDTO);

            result.Should().Be(null);
        }

    }
}
