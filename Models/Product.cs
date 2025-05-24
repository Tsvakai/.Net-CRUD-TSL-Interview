using System.ComponentModel.DataAnnotations;
using RestApiExample.Models.Embeddables;

namespace RestApiExample.Models
{
    public class Product : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Pricing Pricing { get; set; } = new();

        [Required]
        public Inventory Inventory { get; set; } = new();

        [Required]
        public ProductMetadata ProductMetadata { get; set; } = new();

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check for whitespace-only product name
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult(
                    "Product name cannot be whitespace.",
                    new[] { nameof(Name) });
            }

            // Validate owned complex objects
            foreach (var result in ValidateObject(Pricing, nameof(Pricing)))
                yield return result;

            foreach (var result in ValidateObject(Inventory, nameof(Inventory)))
                yield return result;

            foreach (var result in ValidateObject(ProductMetadata, nameof(ProductMetadata)))
                yield return result;
        }

        private static IEnumerable<ValidationResult> ValidateObject(object obj, string memberName)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(obj);
            Validator.TryValidateObject(obj, context, results, validateAllProperties: true);

            foreach (var result in results)
            {
                // Associate the error with the parent property
                yield return new ValidationResult(result.ErrorMessage ?? "Invalid value.", new[] { memberName });
            }
        }
    }
}
