using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

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
        public string OptionsJson
        {
            get => Options is null ? null : JsonSerializer.Serialize(Options);
            set => Options = value is null ? null : JsonSerializer.Deserialize<List<string>>(value);
        }
        [NotMapped]
        public List<string> Options { get; set; }
        public int Answer { get; set; }
    }
}
