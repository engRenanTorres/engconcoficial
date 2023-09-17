
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
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAPITests.Controllers
{
    public class QuestionControllerTest
    {
        private readonly IQuestionService _questionService;
        private readonly ILogger<QuestionController> _logger;
        private readonly QuestionController _questionController;
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
        private readonly Question _question = new()
        {
            Id = 1,
            CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
            LastUpdatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
            Body = "Is this a quetion Test?",
            Answer = 'A',
            Tip = "A",
            CreatedById = 1,
            CreatedBy = null,
        };

        public QuestionControllerTest()
        {
            _questionService = A.Fake<IQuestionService>();
            _logger = A.Fake<ILogger<QuestionController>>();
            _questionController = new QuestionController(_logger, _questionService);
        }

        [Fact]
        public async Task CreateQuestion_ReturnQuetion()
        {
            var newQuestion = new CreateQuestionDTO()
            {
                Body = "This is a quetion Test?",
                Answer= "A",
                Tip= "A",
            };
            var authUserId = "1";
            var httpContext = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", authUserId)
            }));
            httpContext.User = userClaims;
            _questionController.ControllerContext.HttpContext = httpContext;


            A.CallTo(() => _questionService.CreateQuestion(newQuestion,authUserId)).Returns(Task.FromResult<Question?>(_question));

            var result = await _questionController.CreateQuestion(newQuestion);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status201Created);

            createdResult?.Value.Should().BeSameAs(_question);
        }

        [Fact]
        public async Task CreateQuestion_NullDTO_ThrowsBadRequest()
        {
            CreateQuestionDTO? createQuestionDTO = null;

            var actionResult = await _questionController.CreateQuestion(createQuestionDTO);

            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = actionResult.Result as BadRequestObjectResult;

            badRequestResult?.StatusCode.Should().Be(400);

            badRequestResult?.Value.Should().Be("Question data is null.");
        }

        [Fact]
        public async Task CreateQuestion_InvalidUserId_ThrowsNotFound()
        {
            var createQuestionDTO = new CreateQuestionDTO();
            var actionResult = await _questionController.CreateQuestion(createQuestionDTO);

            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var badRequestResult = actionResult.Result as NotFoundObjectResult;

            badRequestResult?.StatusCode.Should().Be(404);

            badRequestResult?.Value.Should().Be("Please log a user");
        }

        [Fact]
        public async Task CreateQuestion_InvalidData_ThrowsBadRequest()
        {
            var authUserId = "1";
            var httpContext = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", authUserId)
            }));
            httpContext.User = userClaims;
            _questionController.ControllerContext.HttpContext = httpContext;
            var createQuestionDTO = new CreateQuestionDTO();

            A.CallTo(() => _questionService.CreateQuestion(createQuestionDTO, "1")).Returns(Task.FromResult<Question?>(null));

            var actionResult = await _questionController.CreateQuestion(createQuestionDTO);


            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = actionResult.Result as BadRequestObjectResult;

            badRequestResult?.StatusCode.Should().Be(400);

            badRequestResult?.Value.Should().Be("Question has Not been Created");
        }


        [Fact]
        public async Task GetQuestion_ReturnQuestion()
        {
            A.CallTo(() => _questionService.GetQuestion(1)).Returns(Task.FromResult<Question?>(_question));

            var result = await _questionController.GetQuestion(1);

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(_question);
        }

        [Fact]
        public async Task GetQuestion_NotFoundQuestion_ThrowsNotFound()
        {
            A.CallTo(() => _questionService.GetQuestion(1)).Returns(Task.FromResult<Question?>(null));

            var actionResult = await _questionController.GetQuestion(1);

            actionResult.Result.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var badRequestResult = actionResult.Result as NotFoundObjectResult;

            badRequestResult?.StatusCode.Should().Be(404);

            badRequestResult?.Value.Should().Be("Question id: 1 not found");
        }

        [Fact]
        public async Task GetQuestions_ReturnQuesitons()
        {
            var questions = new List<Question>
            {
                _question
            };
            A.CallTo(() => _questionService.GetAllQuestions()).Returns(Task.FromResult<IEnumerable<Question>>(questions));

            var result = await _questionController.GetQuestions();

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(questions);
        }

        [Fact]
        public async Task PatchUser_ReturnUser()
        {
            var newUserData = new UpdateQuestionDTO();
            /*var authUserId = "1";
            var httpContext = new DefaultHttpContext();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", authUserId)
            }));
            httpContext.User = userClaims;
            _userController.ControllerContext.HttpContext = httpContext;*/


            A.CallTo(() => _questionService.PatchQuestion(1, newUserData))
                .Returns(Task.FromResult<Question?>(_question));

            var result = await _questionController.PatchQuestion(1,newUserData);

            result.Result.Should().BeOfType<OkObjectResult>();
            var createdResult = result.Result as OkObjectResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdResult?.Value.Should().BeSameAs(_question);
        }

        [Fact]
        public async Task DeleteQuestion_ReturnNoContent()
        {
            var authUserId = 1;

            A.CallTo(() => _questionService.DeleteQuestion(1))                
                .Returns(true);

            var result = await _questionController.DeleteQuestion(authUserId);


            var createdResult = result as NoContentResult;

            createdResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task DeleteQuestion_QuestionNotFound_ReturnsNotFound()
        {
            // Arrange
            A.CallTo(() => _questionService.DeleteQuestion(1))
                .Throws(new WarningException());
            // Act
            var result = await _questionController.DeleteQuestion(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>(); 
            var notFoundResult = (NotFoundObjectResult)result;

            notFoundResult.StatusCode.Should().Be(404); 

            notFoundResult.Value.Should().Be("Question id: 1 not found"); 
        }

        [Fact]
        public async Task DeleteQuestion_ExceptionOccurred_ReturnsInternalServerError()
        {
            A.CallTo(() => _questionService.DeleteQuestion(1))
                .Throws(new Exception("Test exception"));

            try
            {
                var result = await _questionController.DeleteQuestion(1);

            } catch (Exception result)
            {
                result.Should().BeOfType<Exception>();
                result.Message.Should()
                    .Be("Error to delete Question");
            }
            
        }

    }
}
