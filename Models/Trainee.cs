using System.Text.Json.Serialization;

namespace TrainingManager.Models
{
    public class Trainee
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("completions")]
        public List<Training> Completions { get; set; }
    }
}
