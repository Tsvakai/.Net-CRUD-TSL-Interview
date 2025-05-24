using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApiExample.Models.Embeddables
{
    public class ProductMetadata
    {
        [Required]
        public string SKU { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Manufacturer { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Warranty { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        [MaxLength(2048)]
        public string? ImageUrl { get; set; }

        [MaxLength(2048)]
        public string? ThumbnailUrl { get; set; }

        public string? Tags { get; set; } // Use comma-separated tags
    }
}
