namespace API_QUIZZ.Models.Dtos
{
    public class ResponseCreateDto
    {
        public string QuestionId { get; set; }
        public int UserAnswer { get; set; }
        public string UserId { get; set; }
    }
}
