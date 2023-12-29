﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pipelights.Database.Models
{
    public class ProductEntity
    {
        public string id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("technicalData")]
        public string TechnicalData { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("pricereduced")]
        public string PriceReduced { get; set; }

        [JsonPropertyName("isInactive")]
        public bool IsInactive { get; set; }

        [JsonPropertyName("isInactive")]
        public bool IsOnStock { get; set; }
        [JsonPropertyName("isService")]
        public bool IsService { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }

    }
}
