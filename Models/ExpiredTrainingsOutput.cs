using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainingManager.Models
{
    public class ExpiredTrainingsOutput
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("completions")]
        public List<ExpiredTraining> Completions { get; set; }
    }

    public class ExpiredTraining
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        [JsonPropertyName("status")]
        public string TrainingStatus { get; set; }
        [JsonPropertyName("expires")]
        public string Expires { get; set; }
    }
}
