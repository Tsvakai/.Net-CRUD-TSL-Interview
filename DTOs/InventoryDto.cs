using System.ComponentModel.DataAnnotations;

namespace RestApiExample.DTOs
{
    public class InventoryDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be zero or more.")]
        public int QuantityInStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be zero or more.")]
        public int ReorderLevel { get; set; }
    }
}
