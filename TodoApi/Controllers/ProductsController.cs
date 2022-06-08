using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using Serilog;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Products controller that exposes the endpoints to interact with product data
    /// </summary>
    [Route("Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Exposes the endpoint to retrieve products based on optional query parameters
        /// </summary>
        /// <param name="sku">Optional string</param>
        /// <param name="type">Optional string</param>
        /// <param name="name">Optional string</param>
        /// <param name="description">Optional string</param>
        /// <param name="manufacturer">Optional string</param>
        /// <param name="price">Optional string</param>
        /// <returns>Products</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts(string? sku = null, string? type = null, string? name = null, string? description = null, string? manufacturer = null, string? price = null)
        {
            Log.Information("Request received for Get Products");
            return Ok(_productService.GetProductsByQuery(sku, price, name, description, manufacturer, type));
        }

        /// <summary>
        /// Exposes the endpoint to retrieve a product by its id
        /// </summary>
        /// <param name="id">Int</param>
        /// <returns>Product</returns>
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            Log.Information("Request received for Get product by id");
            return _productService.GetProductById(id);
        }

        /// <summary>
        /// Exposes the endpoint to create a new product
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Product</returns>
        [HttpPost]
        public ActionResult<Product> PostProduct(Product product)
        {
            Log.Information("Request received create product");
            return _productService.CreateProduct(product);
        }

        /// <summary>
        /// Exposes the endpoint to update an existing product
        /// </summary>
        /// <param name="id">Int, path parameter</param>
        /// <param name="product">Product</param>
        /// <returns>Product</returns>
        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(int id, Product product)
        {
            Log.Information("Request received for update product");
            return _productService.UpdateProduct(id, product);
        }

        /// <summary>
        /// Exposes the endpoint to delete a product by its id
        /// </summary>
        /// <param name="id">int, path parameter</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            Log.Information("Request received for delete product");
            return _productService.DeleteProduct(id);
        }
    }
}