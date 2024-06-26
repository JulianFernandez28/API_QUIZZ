using System.Net;

namespace API_QUIZZ.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<String> ErrorMessages { get; set; }
        public object Resultado { get; set; }
        public int TotalPaginas { get; set; }
    }
}
