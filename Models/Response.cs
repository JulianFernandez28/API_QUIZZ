using System.ComponentModel.DataAnnotations;

namespace API_QUIZZ.Models
{
    public class Response
    {
        [Key]
        public int Id { get; set; }
        public string QuestionId { get; set; }
        public int UserAnswer { get; set; }
        public string UserId { get; set; }
    }
}
