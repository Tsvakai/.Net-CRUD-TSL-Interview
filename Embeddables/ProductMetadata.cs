using System.ComponentModel.DataAnnotations;

namespace RestApiExample.Models.Embeddables
{
    public class ProductMetadata
    {
        [Required]
        public string SKU { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? ThumbnailUrl { get; set; }

        public List<string>? Tags { get; set; }
    }
}
