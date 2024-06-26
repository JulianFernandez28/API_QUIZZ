

using API_QUIZZ.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace API_QUIZZ.Services
{
    public class QuizzService
    {
        private static string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        private static List<Questions> _questions;
        private static List<Response> _response;
        private static List<User> _users;

        public QuizzService()
        {
            if(!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);

            }

            if(!File.Exists(Path.Combine(dataPath, "questions.json"))) 
            {
                File.Create(Path.Combine(dataPath, "questions.json")).Close();
                _questions = new List<Questions>();

            }
            else
            {
                var json = File.ReadAllText(Path.Combine(dataPath, "questions.json"));
                _questions =  JsonConvert.DeserializeObject<List<Questions>>(json);
                if (_questions is null)
                {
                    _questions = new List<Questions>();
                }
            }

            if (!File.Exists(Path.Combine(dataPath, "responses.json")))
            {
                File.Create(Path.Combine(dataPath, "responses.json")).Close();
                _response = new List<Response>();

            }
            else
            {
                var json = File.ReadAllText(Path.Combine(dataPath, "response.json"));
                _response = JsonConvert.DeserializeObject<List<Response>>(json);
                if (_response is null)
                {
                    _response = new List<Response>();
                }
            }

            if (!File.Exists(Path.Combine(dataPath, "users.json")))
            {
                File.Create(Path.Combine(dataPath, "users.json")).Close();
                _users = new List<User>();

            }
            else
            {
                var json = File.ReadAllText(Path.Combine(dataPath, "users.json"));
                _users = JsonConvert.DeserializeObject<List<User>>(json);
                if (_users is null)
                {
                    _users = new List<User>();
                }
           
            }
        }


        public  IEnumerable<Questions> GetQuestions()
        {
            var random = new Random();

            return   _questions.OrderBy(q => random.Next()).Take(10).ToList();
        }

        public User GetUser(string id)
        {
            return _users.FirstOrDefault(u => u.Id.Equals(id));
        }

        public List<User> GetUsers()
        {
            return _users.OrderByDescending(u => u.Score).ToList();
        }

        public async Task<bool> SaveResponse(Response response)
        {
            _response.Add(response);

            await File.WriteAllTextAsync(Path.Combine(dataPath, "responses.json"),JsonConvert.SerializeObject(_response));

            var question = _questions.Find(q => q.Question == response.QuestionText);

            if(question != null && question.Answer == response.UserAnswer)
            {
                var user = _users.Find( u => u.Id == response.UserId );

                if(user != null)
                {
                    user.Score++;
                    await File.WriteAllTextAsync(Path.Combine(dataPath,"users.json"), JsonConvert.SerializeObject(_users));
                    return true;
                }
            }

            return false;

        }

        public async Task SaveUser(UserDTO user)
        {
            User newUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = user.Name,
                Score = 0
            };

            
            
            _users.Add(newUser);

            await File.WriteAllTextAsync(Path.Combine(dataPath, "users.json"), JsonConvert.SerializeObject(_users));
        }


    }
}
