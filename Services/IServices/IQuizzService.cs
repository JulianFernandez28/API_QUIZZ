using API_QUIZZ.Models;
using API_QUIZZ.Models.Dtos;

namespace API_QUIZZ.Services.IServices
{
    public interface IQuizzService
    {
        Task<Questions> CreateQuestion(Questions question);
        Task<IEnumerable<Questions>> GetQuestions();
        Task<Questions> GetQuestion(string id);
        Task<bool> SaveResponse(Response response);
        Task<User> SaveUser(User user);
        Task<User> GetUser(string id);
        Task<IEnumerable<User>> GetUsers();
    }
}
