using API_QUIZZ.Models;
using API_QUIZZ.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace API_QUIZZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzController : ControllerBase
    {
        private QuizzService _quizzService;
        private APIResponse _response;

        public QuizzController()
        {
            _quizzService = new QuizzService();
            _response = new APIResponse();
        }

        //https://localhost:7169/api/Quizz/questions
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
        }

    }
}
