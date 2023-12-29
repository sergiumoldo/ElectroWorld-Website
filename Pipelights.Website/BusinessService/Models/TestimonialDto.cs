using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pipelights.Database.Models
{
    public class TestimonialDto
    {
        public string id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsInactive { get; set; }

        public int Stars { get; set; }

    }
}
