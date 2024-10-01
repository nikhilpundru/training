using System.Text.Json.Serialization;

namespace TrainingManager.Models
{
    public class Training
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        public DateTime? CompletionDate
        {
            get
            {
                if(this.Timestamp != null)
                {
                    return DateTime.ParseExact(this.Timestamp, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                return null;
            }

        }

        public DateTime? ExpiresOn
        {
            get
            {
                if(this.Expires != null)
                {
                    return DateTime.ParseExact(this.Expires, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                return null;
            }
        }

        [JsonPropertyName("expires")]
        public string Expires { get; set; }
    }
}
