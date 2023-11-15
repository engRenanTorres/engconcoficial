using DotnetAPI.DTOs;
using DotnetAPI.Models.Inharitance;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Services
{
    public interface IQuestionService
    {
        Task<BaseQuestion?> CreateQuestionOrThrow(CreateQuestionDTO questionDTO, string userId);
        Task<BaseQuestion?> GetQuestion(int id);
        Task<IEnumerable<BaseQuestion?>> GetAllQuestions();
        Task<BaseQuestion> PatchQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO);
        Task<bool> DeleteQuestion(int id);
    }
}