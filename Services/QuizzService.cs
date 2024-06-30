

using API_QUIZZ.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using API_QUIZZ.Services.IServices;
using API_QUIZZ.Data;
using AutoMapper;
using API_QUIZZ.Models.Dtos;
using Org.BouncyCastle.Asn1.Ocsp;

namespace API_QUIZZ.Services
{
    public class QuizzService:IQuizzService
    {
        private readonly ApplicationDbContext _context;


        public QuizzService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Questions> CreateQuestion(Questions question)
        {
            question.Id = Guid.NewGuid().ToString();
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return question;
        }

        public async Task<IEnumerable<Questions>> GetQuestions()
        {
            var random = new Random();

            IEnumerable<Questions> list = await _context.Questions.ToListAsync();

            IEnumerable<Questions> listRandom = list.OrderBy(q => random.Next()).Take(10);

            return listRandom;

        }
        
        public async Task<Questions> GetQuestion(string id)
        {
            Questions question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
            return question;
        }

        public async Task<bool> SaveResponse(Response response)
        {

            await _context.Responses.AddAsync(response);

            Questions question = await GetQuestion(response.QuestionId);

          
            if (question.Answer == response.UserAnswer)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == response.UserId);
                user.Score++;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;

            }
               
            await _context.SaveChangesAsync();
            return false;

        }

        public async Task<User> SaveUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;

        }

        public Task<User> GetUser(string id)
        {
            var user = _context.Users.FirstOrDefaultAsync( u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            IEnumerable<User> listUser = await _context.Users.ToListAsync();
            return listUser;
        }
    }
}
