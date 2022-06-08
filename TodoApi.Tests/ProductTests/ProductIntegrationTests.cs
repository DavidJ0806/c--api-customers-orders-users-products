using Xunit;
using TodoApi.Controllers;
using TodoApi.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using TodoApi.Services;
using TodoApi.Repositories;
using TodoApi.Models;

public class ProductControllerTests
{
  private async Task<ApiDb> GetDatabaseContext()
  {
    var options = new DbContextOptionsBuilder<ApiDb>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
    var databaseContext = new ApiDb(options);
    databaseContext.Database.EnsureCreated();
    return databaseContext;
  }
  private async Task<ProductsController> GetProductController()
  {
    var context = await GetDatabaseContext();
    var repository3 = new ProductRepository(context);
    var productService = new ProductService(repository3);
    var productController = new ProductsController(productService);
    return productController;
  }
  [Fact]
  public void GetProducts_AllProducts_Returns200()
  {
    var controller = GetProductController().Result;

    var products = controller.GetProducts();
    var results = (OkObjectResult?)products.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void GetProductById_ValidId_Returns200()
  {
    var controller = GetProductController().Result;

    var products = controller.GetProduct(1);
    var results = (OkObjectResult?)products.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void GetProductById_InvalidId_Returns404()
  {
    var controller = GetProductController().Result;

    var products = controller.GetProduct(100);
    var results = (NotFoundResult?)products.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
  [Fact]
  public void PostProduct_ValidProduct_Returns201()
  {
    var controller = GetProductController().Result;

    var products = controller.PostProduct(new Product { Name = "helol", Sku = "wwdaowda", Type = "brown", Description = "123", Manufacturer = "coke", Price = 12.12m });
    var results = (CreatedResult?)products.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(201, statusCode);
  }
  [Fact]
  public void PostProduct_SkuTaken_Returns409()
  {
    var controller = GetProductController().Result;

    var products = controller.PostProduct(new Product { Name = "helol", Sku = "23451", Type = "brown", Description = "123", Manufacturer = "coke", Price = 12.12m });
    var results = (ConflictObjectResult?)products.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(409, statusCode);
  }
  [Fact]
  public void PutProduct_SkuTaken_Returns409()
  {
    var controller = GetProductController().Result;

    var products = controller.UpdateProduct(1, new Product { Id = 1, Name = "helol", Sku = "29459", Type = "brown", Description = "123", Manufacturer = "coke", Price = 12.12m });
    var results = (ConflictResult?)products.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(409, statusCode);
  }
  [Fact]
  public void PutProduct_ProductDoesNotExistInDatabase_Returns404()
  {
    var controller = GetProductController().Result;

    var products = controller.UpdateProduct(10, new Product { Id = 10, Name = "helol", Sku = "29459", Type = "brown", Description = "123", Manufacturer = "coke", Price = 12.12m });
    var results = (NotFoundResult?)products.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
  [Fact]
  public void PutProduct_ValidProduct_Returns200()
  {
    var controller = GetProductController().Result;

    var products = controller.UpdateProduct(8, new Product { Id = 8, Name = "helol", Sku = "292323459", Type = "brown", Description = "123", Manufacturer = "coke", Price = 12.12m });
    var results = (OkObjectResult?)products.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void DeleteProduct_ValidProduct_Returns204()
  {
    var controller = GetProductController().Result;

    var products = controller.DeleteProduct(8);
    var results = (NoContentResult?)products;
    var statusCode = results?.StatusCode;

    Assert.Equal(204, statusCode);
  }
  [Fact]
  public void DeleteProduct_ProductDoesntExist_Returns404()
  {
    var controller = GetProductController().Result;

    var products = controller.DeleteProduct(80);
    var results = (NotFoundResult?)products;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
}