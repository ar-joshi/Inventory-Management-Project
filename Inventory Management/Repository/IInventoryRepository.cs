using Inventory_Management.Models.DTOs;
using System.Collections.Generic;


namespace Inventory_Management.Repository
{
    public interface IInventoryRepository
    {
        IEnumerable<ProductDto> GetProducts();

        IEnumerable<ProductDto> GetProduct(int id);

        void AddProduct(ProductDto productDto);

        void DeleteProduct(int id);

        void UpdateProduct(ProductDto productDto);
    }
}