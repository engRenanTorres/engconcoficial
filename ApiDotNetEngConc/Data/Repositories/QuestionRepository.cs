using DotnetAPI.Models.Inharitance;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data.Repositories;

class QuestionRepository : IQuestionRepository
{
    private readonly DataContextEF _context;
    public QuestionRepository(DataContextEF context)
    {
        _context = context;
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void AddEntity<BaseQuestion>(BaseQuestion entity)
    {
        if (entity != null)
        {
            _context.Add(entity);
        }
    }

    public void RemoveEntity<BaseQuestion>(BaseQuestion entity)
    {
        if (entity != null) { _context.Remove(entity); }
    }

    public async Task<IEnumerable<BaseQuestion?>> GetAllQuestions()
    {
        if (_context.Questions != null)
        {
            IEnumerable<BaseQuestion?> questions = await _context.Questions
              .Include(q => q.CreatedBy)
              .AsQueryable()
              .ToListAsync();

            return questions;
        }
        throw new Exception("Questions repo is not set");
    }
    public async Task<BaseQuestion?> GetSingleQuestion(int id)
    {
        if (_context.Questions != null)
        {
            BaseQuestion? question = await _context.Questions
              .Include(q => q.CreatedBy)
              .FirstOrDefaultAsync(u => u.Id == id);
            return question;
        }
        throw new Exception("Questions repo is not set");
    }
}