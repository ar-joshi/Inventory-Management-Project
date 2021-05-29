using DataAccessLayer.Models;
using Inventory_Management.Models.DTOs;
using System.Linq;

namespace Inventory_Management.Utility
{
    public static class ProductValidation
    {
        //Functions to validate the product info from Request Body
        public static ProductValidationEnum ValidateAddProduct(ProductDto productDto)
        {
            using (var context = new InventoryDBEntities())
            {
                //product category validation check
                if(!context.ProductCategories.Select(x => x.Name.ToLower()).Contains(productDto.CategoryName.ToLower()))
                {
                    return ProductValidationEnum.InvalidCategory;
                }
                //product name validation check
                else if (context.Products.Select(x => x.Name.ToLower()).Contains(productDto.Name.ToLower()))
                {
                    return ProductValidationEnum.InvalidName;
                }
                else if (!productDto.Price.HasValue) //product price validation
                {
                    return ProductValidationEnum.InvalidPrice;
                }
                else if (string.IsNullOrEmpty(productDto.BrandName)) //brand name validation
                {
                    return ProductValidationEnum.InvalidBrandName;
                }
                else
                {
                    return ProductValidationEnum.ValidData;
                }
            }
        }

        public static ProductValidationEnum ValidateProduct(int productId)
        {
            using (InventoryDBEntities context = new InventoryDBEntities())
            {
                //product id check
                if (!context.Products.Select(x => x.ProductId).Contains(productId))
                {
                    return ProductValidationEnum.InvalidProduct;
                }

                return ProductValidationEnum.ValidData;
            }
        }

        //Validation for UpdateProduct request
        public static ProductValidationEnum ValidateUpdateProduct(ProductDto productDto)
        {
            using (InventoryDBEntities context = new InventoryDBEntities())
            {
                if (!context.Products.Select(x => x.ProductId).Contains(productDto.ProductId))
                {
                    return ProductValidationEnum.InvalidProduct;
                }
                else if (string.IsNullOrEmpty(productDto.CategoryName) || string.IsNullOrEmpty(productDto.Name))
                {
                    return ProductValidationEnum.ValidData;
                }
                else if (!context.ProductCategories.Select(x => x.Name.ToLower()).Contains(productDto.CategoryName.ToLower()))
                {
                    return ProductValidationEnum.InvalidCategory;
                }

                return ProductValidationEnum.ValidData;
            }
        }
    }
}