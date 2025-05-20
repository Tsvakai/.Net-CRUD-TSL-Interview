using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApiExample.DTOs
{
    public class ProductDto : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public PricingDto Pricing { get; set; } = new();

        [Required]
        public InventoryDto Inventory { get; set; } = new();

        [Required]
        public ProductMetadataDto Metadata { get; set; } = new();

        public bool IsActive { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Product name cannot be empty or whitespace.", new[] { nameof(Name) });
            }

            var pricingResults = new List<ValidationResult>();
            Validator.TryValidateObject(Pricing, new ValidationContext(Pricing), pricingResults, true);
            foreach (var result in pricingResults) yield return result;

            var inventoryResults = new List<ValidationResult>();
            Validator.TryValidateObject(Inventory, new ValidationContext(Inventory), inventoryResults, true);
            foreach (var result in inventoryResults) yield return result;

            var metadataResults = new List<ValidationResult>();
            Validator.TryValidateObject(Metadata, new ValidationContext(Metadata), metadataResults, true);
            foreach (var result in metadataResults) yield return result;
        }
    }
}
