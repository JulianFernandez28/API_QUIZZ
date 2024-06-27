namespace API_QUIZZ.Models
{
    public class Questions
    {
        public Questions()
        {
            Options = new List<string>();
        }
        public string Id { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int Answer { get; set; }
    }
}
