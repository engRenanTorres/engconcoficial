using DotnetAPI.DTOs;
using DotnetAPI.Models.Inharitance;

namespace DotnetAPI.Data.Repositories;
public interface IQuestionRepository
{
    public Task<bool> SaveChanges();
    public void AddEntity<BaseQuestion>(BaseQuestion entity);
    public void RemoveEntity<BaseQuestion>(BaseQuestion entity);
    public Task<IEnumerable<BaseQuestion?>> GetAllQuestions();
    public Task<BaseQuestion?> GetSingleQuestion(int id);
}