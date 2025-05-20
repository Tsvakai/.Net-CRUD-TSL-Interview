using System.ComponentModel.DataAnnotations;

namespace RestApiExample.DTOs
{
    public class PricingDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Discount price must be zero or positive.")]
        public decimal? DiscountPrice { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3-letter ISO code.")]
        public string Currency { get; set; } = "USD";
    }
}
