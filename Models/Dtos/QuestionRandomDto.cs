using System.ComponentModel.DataAnnotations.Schema;

namespace API_QUIZZ.Models.Dtos
{
    public class QuestionRandomDto
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int Answer { get; set; }
    }
}
