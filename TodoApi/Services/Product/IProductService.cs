using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetProductsByQuery(string sku, string price, string name, string description, string manufacturer, string type);
        ActionResult<Product> GetProductById(int id);
        ActionResult<Product> CreateProduct(Product product);
        ActionResult<Product> UpdateProduct(int id, Product product);
        ActionResult DeleteProduct(int id);
    }
}