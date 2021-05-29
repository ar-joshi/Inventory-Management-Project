using System.ComponentModel.DataAnnotations;

namespace Inventory_Management.Models.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string BrandName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public int? AvailableQuantity { get; set; }

        public bool? IsActive { get; set; }
    }
}
