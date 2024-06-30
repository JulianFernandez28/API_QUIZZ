using API_QUIZZ.Data;
using API_QUIZZ.Models;
using API_QUIZZ.Models.Dtos;
using API_QUIZZ.Services;
using API_QUIZZ.Services.IServices;
using AutoMapper;
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
        
        private APIResponse _response;
        private readonly ApplicationDbContext _context;
        private readonly IQuizzService _quizzService;
        private readonly IMapper _mapper;


        public QuizzController(ApplicationDbContext context, IMapper mapper, IQuizzService quizzService)
        {

            _response = new APIResponse();
            _context = context;
            _mapper = mapper;
            _quizzService = quizzService;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> Post(Questions questions)
        {
            try
            {
                if (questions == null)
                {
                    _response.IsExitoso= false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("La pregunta no puede estar vacia");
                    
                    return BadRequest(_response);
                }

                _response.Resultado = await _quizzService.CreateQuestion(questions);
                _response.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {

                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return _response;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Questions>>> Get()
        {
            IEnumerable<Questions> list = await _context.Questions.ToListAsync();

            return Ok(list);
        }

        [HttpGet("questions")]
        public async Task<ActionResult<APIResponse>> GetRandom()
        {
            try
            {
                var ListQuestion = await _quizzService.GetQuestions();

                _response.Resultado = _mapper.Map<IEnumerable<QuestionDto>>(ListQuestion);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.StatusCode= HttpStatusCode.InternalServerError;
               
            }
            return _response;
        }

        [HttpPost("response")]
        public  async Task<ActionResult<APIResponse>> PostQuestion([FromBody]ResponseCreateDto  responseDto)
        {
            try
            {

                var response = _mapper.Map<Response>(responseDto);

                var question = await _quizzService.GetQuestion(responseDto.QuestionId);

                if (question is null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso=false;
                    return NotFound(_response);
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
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }


            return _response;

            
        }

        //https://localhost:7169/api/Quizz/user
        [HttpPost("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> PostUser([FromBody] UserDto newUser)
        {
            
            try
            {
                if(string.IsNullOrEmpty(newUser.Name)){
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("El campo name no puede ser vacio");
                    _response.IsExitoso = true;
                    return BadRequest(_response);
                }
                var user = _mapper.Map<User>(newUser);

                if (user is null)
                {
                    _response.IsExitoso = false;
                    _response.ErrorMessages.Add("Error al cargar el usuario");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _response.Resultado =await _quizzService.SaveUser(user);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return _response;
        }



        //https://localhost:7169/api/Quizz/allUser
        [HttpGet("allUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            try
            {
                _response.Resultado = await _quizzService.GetUsers();
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
               
            }

            return _response;
        }

        //https://localhost:7169/api/Quizz/user/ + "id"
        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add($"El id: {id} es incorrecto");
            }
            try
            {
                _response.Resultado = await _quizzService.GetUser(id);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return _response;
        }

    }
}
