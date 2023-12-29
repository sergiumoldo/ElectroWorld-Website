using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pipelights.Database.Models
{
    public class TestimonialEntity
    {
        public string id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("isInactive")]
        public bool IsInactive { get; set; }

        [JsonPropertyName("stars")]
        public int Stars { get; set; }

    }
}
