using API_QUIZZ.Data;
using API_QUIZZ.Models;
using API_QUIZZ.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;

namespace API_QUIZZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzController : ControllerBase
    {
        private QuizzService _quizzService;
        private APIResponse _response;
        private readonly ApplicationDbContext _context;


        public QuizzController(ApplicationDbContext context)
        {
            _quizzService = new QuizzService();
            _response = new APIResponse();
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Questions>> Post(Questions questions)
        {
            try
            {
                _context.Questions.Add(questions);
                await _context.SaveChangesAsync();
                    
                return Ok(questions);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Questions>>> Get()
        {
            IEnumerable<Questions> list = await _context.Questions.ToListAsync();

            return Ok(list);
        }

        [HttpGet("questions")]
        public async Task<ActionResult<IEnumerable<Questions>>> GetRandom()
        {
            var random = new Random();

            IEnumerable<Questions> list = await _context.Questions.ToListAsync();

            IEnumerable<Questions> listRandom = list.OrderBy(q => random.Next()).Take(10);

            return Ok(listRandom);
        }

        [HttpPost("response")]
        public  async Task<ActionResult<Response>> PostQuestion(Response response)
        {
            try
            {
                var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == response.QuestionId);

                if (question is null)
                {
                    return BadRequest("Pregunta no encontrada");
                }

                User user = await _context.Users.FirstOrDefaultAsync( u => u.Id == response.UserId);

                if(user is null)
                {
                    return BadRequest("Usuario no existe");
                }

                if (question.Answer == response.UserAnswer)
                {
                    user.Score++;
                }
                await _context.Responses.AddAsync(response);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(response);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }

            
        }

        [HttpPost("user")]
        public async Task<ActionResult<User>> PostUser(User newUser)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == newUser.Name);

                if(user != null )
                {
                    return BadRequest("Ingrese otro nombre");
                }

                newUser.Id = Guid.NewGuid().ToString();

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();


                return Ok(newUser);
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
            
        }

        [HttpGet("alluser")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {

            try
            {
                IEnumerable<User> listUser = await _context.Users.ToListAsync();

                if (listUser is null)
                {
                    return BadRequest("Lista vacia");
                }

                return Ok(listUser);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
            
        }



        /*//https://localhost:7169/api/Quizz/questions
        [HttpGet("questions")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<APIResponse> Get()
        {
            try
            {
                _response.Resultado =  _quizzService.GetQuestions();
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

               _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;

            }

            return Ok(_response);
        }

        //https://localhost:7169/api/Quizz/response
        [HttpPost("response")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> Post([FromBody] Response response)
        {
            try
            {
                if (response is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso =false;
                    _response.ErrorMessages.Add("Error al cargar la respuesta");
                }
                var answer = await _quizzService.SaveResponse(response);

                if (!answer)
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Resultado = "Wrong";
                    _response.IsExitoso = false;
                    return _response;
                }
                _response.Resultado = "Well Done!";
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                _response.IsExitoso=false;
                _response.ErrorMessages = new List<string> { ex.Message};
                _response.StatusCode=HttpStatusCode.InternalServerError;
            }

            return Ok(_response);
        }


        //https://localhost:7169/api/Quizz/user
        [HttpPost("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> PostUser([FromBody] UserDTO user)
        {
            if(user is null)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("Error al cargar el usuario");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {

                await _quizzService.SaveUser(user);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                _response.IsExitoso =false;
                _response.ErrorMessages = new List<string>() { ex.Message};
                _response.StatusCode=HttpStatusCode.InternalServerError;
            }

            return Ok(_response);
        }
        //https://localhost:7169/api/Quizz/user/ + "id"
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public  ActionResult<APIResponse> Get(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add($"El id: {id} es incorrecto");
            }
            try
            {
                _response.Resultado = _quizzService.GetUser(id);
                _response.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages=new List<string>() { ex.Message};
                _response.StatusCode=HttpStatusCode.InternalServerError;
            }

            return Ok(_response);
        }


        //https://localhost:7169/api/Quizz/allUser
        [HttpGet("allUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<APIResponse> GetUsers()
        {
            try
            {
                _response.Resultado = _quizzService.GetUsers();
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso =false;
                _response.ErrorMessages = new List<string> { ex.Message};
                _response.StatusCode=HttpStatusCode.InternalServerError;
                throw;
            }

            return Ok(_response);
        }*/

    }
}
