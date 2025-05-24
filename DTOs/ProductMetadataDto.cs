using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApiExample.DTOs
{
    public class ProductMetadataDto
    {
        [Required]
        public string SKU { get; set; } = string.Empty;
        
        [Required]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        public string Warranty { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        public string? ImageUrl { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string? Tags { get; set; }
    }
}
