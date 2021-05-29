using Inventory_Management.Helpers;
using Inventory_Management.Models.DTOs;
using Inventory_Management.Repository;
using Inventory_Management.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Inventory_Management.Controllers
{
    public class ProductController : ApiController
    {

        private IInventoryRepository _inventoryRepository;

        //Use of constructor dependency injection | DI Framework used: Ninject
        public ProductController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }


        [Route("api/v1/products")]
        [Authorize]
        public HttpResponseMessage GetAllProducts()
        {
            try
            {
                //gets all the products from Products table
                IEnumerable<ProductDto> result = _inventoryRepository.GetProducts();

                //Log required information to the Log file. (Logging will be done to DB in future.)
                LogHelper.GetInstance().Info("Get all products successful!");

                return result.Any()
                    ? Request.CreateResponse(HttpStatusCode.OK, result)
                    : Request.CreateErrorResponse(HttpStatusCode.NotFound, "No products found");
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("Exception: Get all products!", ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                throw;
            }
        }

        [Route("api/v1/products/{id}")]
        [Authorize]
        public HttpResponseMessage GetProduct(int id)
        {
            try
            {
                //gets product info using product id
                IEnumerable<ProductDto> result = _inventoryRepository.GetProduct(id);

                return result.Any()
                    ? Request.CreateResponse(HttpStatusCode.OK, result)
                    : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product not found");
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("Exception: Get product by id!", ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                throw;
            }
        }

        [Route("api/v1/products")]
        [HttpPost, Authorize]
        public HttpResponseMessage AddProduct([FromBody]ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //call product validation function
                    var validProductCheck = ProductValidation.ValidateAddProduct(productDto);

                    //check if product category is present in dbo.productcategories table
                    if (validProductCheck == ProductValidationEnum.InvalidCategory)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter valid category.");
                    }
                    //restrict if product name is already present in dbo.products table, as we don't want duplicate names
                    //here we can call update method if needed.
                    else if (validProductCheck == ProductValidationEnum.InvalidName)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product Name already exists");
                    }
                    //validation for brand name
                    else if (validProductCheck == ProductValidationEnum.InvalidBrandName)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter valid brand name.");
                    }
                    //validation for price
                    else if (validProductCheck == ProductValidationEnum.InvalidPrice)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter valid price.");
                    }
                    else
                    {
                        //call Add method in InventoryRepository to add record to Products table
                        _inventoryRepository.AddProduct(productDto);
                        LogHelper.GetInstance().Info("Product addition successful");
                        return Request.CreateResponse(HttpStatusCode.OK, "Product added succesfully");
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.GetInstance().Error("Exception: Add product!", ex.Message);
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                    throw;
                }
            }

            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request Body. Please send valid data");
            }

        }

        [Route("api/v1/updateproduct")]
        [HttpPost, Authorize]
        public HttpResponseMessage UpdateProduct([FromBody]ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var validProductCheck = ProductValidation.ValidateUpdateProduct(productDto);

                    //Check if provided product id is present in dbo.products table
                    if (validProductCheck == ProductValidationEnum.InvalidProduct)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product Id is invalid");
                    }

                    //check if product category is present in dbo.productcategories table
                    else if (validProductCheck == ProductValidationEnum.InvalidCategory)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter valid category.");
                    }

                    //call update method in InventoryRepository class
                    _inventoryRepository.UpdateProduct(productDto);

                    LogHelper.GetInstance().Info("Update Successful, Product Id:" + productDto.ProductId);
                    return Request.CreateResponse(HttpStatusCode.OK, "Product details updated succesfully");
                }
                catch (Exception ex)
                {
                    LogHelper.GetInstance().Error("Exception: Update product!", ex.Message);
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                    throw;
                }
            }

            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request Body. Please send valid data");
            }
        }

        [Route("api/v1/deleteproduct/{id}")]
        [HttpDelete, Authorize]
        public HttpResponseMessage DeleteProduct(int id)
        {
            try
            {
                var validProductCheck = ProductValidation.ValidateProduct(id);

                //check if provided product id matches with product id in dbo.Products table
                if (validProductCheck == ProductValidationEnum.InvalidProduct)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Product Id");
                }

                //Soft delete- Set IsActive to false
                _inventoryRepository.DeleteProduct(id);

                LogHelper.GetInstance().Info("Deleted Successfully, Product Id:" + id);
                return Request.CreateResponse(HttpStatusCode.OK, "Product deleted succesfully");
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("Exception: Delete product!", ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                throw;
            }
        }
    }
}
