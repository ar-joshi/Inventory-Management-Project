using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventory_Management.Helpers;
using Inventory_Management.Models.DTOs;

namespace Inventory_Management.Repository
{
    public class InventoryRepository : IInventoryRepository
    {

        //Get all products
        public IEnumerable<ProductDto> GetProducts()
        {
            try
            {
                using (InventoryDBEntities context = new InventoryDBEntities())
                {
                    var res = (from p in context.Products
                               select new
                               {
                                   p.ProductId,
                                   p.Name,
                                   p.BrandName,
                                   p.CategoryId,
                                   p.Description,
                                   p.Price,
                                   p.AvailableQuantity,
                                   p.IsActive
                               }).ToList()
               .Select(x => new ProductDto
               {
                   ProductId = x.ProductId,
                   Name = x.Name,
                   BrandName = x.BrandName,
                   CategoryId = x.CategoryId,
                   Description = x.Description,
                   Price = x.Price,
                   AvailableQuantity = x.AvailableQuantity,
                   IsActive = x.IsActive
               });

                    return res;
                }
            }

            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("Service Exception: Get all products!", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //Get product by Id
        public IEnumerable<ProductDto> GetProduct(int id)
        {
            try
            {
                using (InventoryDBEntities context = new InventoryDBEntities())
                {
                    var res = (from p in context.Products
                           .Where(p => p.ProductId == id)
                               select new
                               {
                                   p.ProductId,
                                   p.Name,
                                   p.BrandName,
                                   p.CategoryId,
                                   p.Description,
                                   p.Price,
                                   p.AvailableQuantity,
                                   p.IsActive
                               }).ToList()
           .Select(x => new ProductDto
           {
               ProductId = x.ProductId,
               Name = x.Name,
               BrandName = x.BrandName,
               CategoryId = x.CategoryId,
               Description = x.Description,
               Price = x.Price,
               AvailableQuantity = x.AvailableQuantity,
               IsActive = x.IsActive
           });

                    return res;
                }

            }

            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("Service Exception: Get product by id!", ex.Message);
                throw new Exception(ex.Message);
            }

        }

        //Add new product to the Inventory
        public void AddProduct(ProductDto productDto)
        {
            try
            {
                using (var context = new InventoryDBEntities())
                {
                    var productCategory = context.ProductCategories.ToList();
                    var productCategoryId = productCategory.Where(x => x.Name == productDto.CategoryName).Select(p => p.CategoryId).FirstOrDefault();

                    var productObj = new Product()
                    {
                        Name = productDto.Name,
                        CategoryId = productCategoryId,
                        BrandName = productDto.BrandName,
                        Price = productDto.Price.Value,
                        AvailableQuantity = productDto.AvailableQuantity ?? 0,
                        Description = productDto.Description,
                        IsActive = productDto.IsActive ?? false,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    context.Products.Add(productObj);
                    context.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("Service Exception: Add new product!", ex.Message);
                throw new Exception(ex.Message);
            }

        }

        //Modify an existing product
        public void UpdateProduct(ProductDto productDto)
        {
            try
            {
                using (InventoryDBEntities context = new InventoryDBEntities())
                {
                    //get category id using provided category name in the req. body
                    var categoryId = context.ProductCategories.Where(x => x.Name == productDto.CategoryName).Select(p => p.CategoryId).FirstOrDefault();
                    var productInfo = context.Products.Where(x => x.ProductId == productDto.ProductId).FirstOrDefault();
                    productInfo.Name = string.IsNullOrEmpty(productDto.Name) ? productInfo.Name : productDto.Name;
                    productInfo.CategoryId = categoryId;
                    productInfo.BrandName = string.IsNullOrEmpty(productDto.BrandName) ? productInfo.BrandName : productDto.BrandName;
                    productInfo.Price = productDto.Price ?? productInfo.Price;
                    productInfo.AvailableQuantity = productDto.AvailableQuantity ?? productInfo.AvailableQuantity;
                    productInfo.IsActive = productDto.IsActive.Value;
                    productInfo.ModifiedDate = DateTime.Now;
                    context.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("Service Exception: Update product by id!", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //Delete product by product id
        public void DeleteProduct(int id)
        {
            try
            {
                using (var context = new InventoryDBEntities())
                {
                    var productInfo = context.Products.Where(x => x.ProductId == id).FirstOrDefault();
                    productInfo.IsActive = false; //Soft delete, just setting IsActive = false
                    context.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("Service Exception: Delete products!", ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}