using TodoApi.Models;
using TodoApi.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Repositories;
using Serilog;
using TodoApi.Controllers;

namespace TodoApi.Services
{
    /// <summary>
    /// A service to use the dbcontext to interact with data
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        /// <summary>
        /// This method takes in optional parameters to filter the list of products
        /// </summary>
        /// <param name="sku">string</param>
        /// <param name="price">string</param>
        /// <param name="name">string</param>
        /// <param name="description">string</param>
        /// <param name="manufacturer">string</param>
        /// <param name="type">string</param>
        /// <returns>IEnumerable, Products</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database is down</exception>
        public IEnumerable<Product> GetProductsByQuery(string sku, string price, string name, string description, string manufacturer, string type)
        {
            try
            {
                return _productRepository.GetProductsByQuery(sku, price, name, description, manufacturer, type);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// This method handles repository calls to get products by id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Ok result, Product</returns>
        /// <exception cref="DatabaseUnavailableException"></exception>
        public ActionResult<Product> GetProductById(int id)
        {
            Product? product = new Product();
            try
            {
                product = _productRepository.GetProductById(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (product == null) return new NotFoundResult();
            return new OkObjectResult(product);
        }

        /// <summary>
        /// This method calls the repository to post products
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Created result, product</returns>
        /// <exception cref="DatabaseUnavailableException"></exception>
        public ActionResult<Product> CreateProduct(Product product)
        {
            bool skuTaken;
            try
            {
                skuTaken = _productRepository.SkuTaken(product);
                if (skuTaken) return new ConflictObjectResult("");
                _productRepository.PostProduct(product);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            return new CreatedResult(nameof(ProductsController), product);
        }

        /// <summary>
        /// This method handles calls to the repository to update products if they exist
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="product">product</param>
        /// <returns>Ok Result, Product</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when db isn't connected</exception>
        public ActionResult<Product> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return new BadRequestResult();
            Product? existingProduct = new Product();
            bool skuTaken;
            try
            {
                existingProduct = _productRepository.GetProductById(id);
                if (existingProduct == null) return new NotFoundResult();
                skuTaken = _productRepository.SkuTaken(product);
                if (skuTaken) return new ConflictResult();
                _productRepository.UpdateProduct(product);
                return new OkObjectResult(product);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// This method calls the repository to make delete requests for existing products
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>No Content</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database isn't connected</exception>
        public ActionResult DeleteProduct(int id)
        {
            Product? productToDelete = new Product();
            try
            {
                productToDelete = _productRepository.GetProductById(id);
                if (productToDelete == null) return new NotFoundResult();
                _productRepository.DeleteProduct(productToDelete);
                return new NoContentResult();
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }
    }
}