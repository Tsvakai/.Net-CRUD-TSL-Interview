using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApiExample.DTOs
{
    public class ProductMetadataDto
    {
        [Required]
        public string SKU { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? ThumbnailUrl { get; set; }

        public List<string>? Tags { get; set; }
    }
}
