using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using TodoApi.Services;
using TodoApi.Repositories;
using Moq;
using TodoApi.Models;
using TodoApi.Utilities.Exceptions;
using System.Collections.Generic;

public class ProductServiceTests
{
  [Fact]
  public void GetProductsByQuery_GetProductsByQuery_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductsByQuery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
      .Throws(new InvalidOperationException());
    var service = new ProductService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.GetProductsByQuery("2", "2", "2", "2", "2", "2"));
  }
  [Fact]
  public void GetProductsByQuery_GetProductsByQuery_IsListOfProducts()
  {
    List<Product> products = new List<Product>();
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductsByQuery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
      .Returns(products);
    var service = new ProductService(mockRepo.Object);

    var actual = service.GetProductsByQuery("2", "2", "2", "2", "2", "2");
    Assert.Equal(products, actual);
  }
  [Fact]
  public void GetProductsByQuery_GetProductById_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>()))
      .Throws(new InvalidOperationException());
    var service = new ProductService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.GetProductById(4));
  }
  [Fact]
  public void GetProductById_GetProductById_NotFoundResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>()))
      .Returns(value: null);
    var service = new ProductService(mockRepo.Object);

    var actual = service.GetProductById(4);
    Assert.IsType<NotFoundResult>(actual.Result);
  }
  [Fact]
  public void GetProductById_GetProductById_IsOkObjectResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>()))
      .Returns(new Product());
    var service = new ProductService(mockRepo.Object);

    var actual = service.GetProductById(4);
    Assert.IsType<OkObjectResult>(actual.Result);
  }
  [Fact]
  public void CreateProduct_SkuTaken_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.SkuTaken(It.IsAny<Product>()))
      .Throws(new InvalidOperationException());
    var service = new ProductService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.CreateProduct(new Product { }));
  }
  [Fact]
  public void CreateProduct_SkuTaken_ReturnsConflictObjectResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.SkuTaken(It.IsAny<Product>()))
      .Returns(true);
    var service = new ProductService(mockRepo.Object);

    var actual = service.CreateProduct(new Product { });
    Assert.IsType<ConflictObjectResult>(actual.Result);
  }
  [Fact]
  public void CreateProduct_SkuTaken_ReturnsCreatedResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.SkuTaken(It.IsAny<Product>()))
      .Returns(false);
    mockRepo.Setup(x => x.PostProduct(It.IsAny<Product>())).Returns(new Product { });
    var service = new ProductService(mockRepo.Object);

    var actual = service.CreateProduct(new Product { });
    Assert.IsType<CreatedResult>(actual.Result);
  }
  [Fact]
  public void UpdateProduct_MismatchingIds_ReturnsBadRequestResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    var service = new ProductService(mockRepo.Object);

    var actual = service.UpdateProduct(1, new Product { Id = 5 });
    Assert.IsType<BadRequestResult>(actual.Result);
  }
  [Fact]
  public void UpdateProduct_GetProductById_ReturnsNotFoundResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(value: null);
    var service = new ProductService(mockRepo.Object);

    var actual = service.UpdateProduct(1, new Product { Id = 1 });
    Assert.IsType<NotFoundResult>(actual.Result);
  }
    [Fact]
  public void UpdateProduct_SkuTaken_ReturnsConflictResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(new Product { });
    mockRepo.Setup(x => x.SkuTaken(It.IsAny<Product>())).Returns(true);
    var service = new ProductService(mockRepo.Object);

    var actual = service.UpdateProduct(1, new Product { Id = 1 });
    Assert.IsType<ConflictResult>(actual.Result);
  }
      [Fact]
  public void UpdateProduct_UpdateProduct_ReturnsOkObjectResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(new Product { });
    mockRepo.Setup(x => x.SkuTaken(It.IsAny<Product>())).Returns(false);
    mockRepo.Setup(x => x.UpdateProduct(It.IsAny<Product>())).Returns(new Product { });
    var service = new ProductService(mockRepo.Object);

    var actual = service.UpdateProduct(1, new Product { Id = 1 });
    Assert.IsType<OkObjectResult>(actual.Result);
  }
   [Fact]
  public void UpdateProduct_GetProductById_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).Throws(new InvalidOperationException());
    var service = new ProductService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateProduct(1, new Product { Id = 1 }));
  }
  [Fact]
  public void DeleteProduct_GetProductById_NotFoundResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(value: null);
    var service = new ProductService(mockRepo.Object);

    var actual = service.DeleteProduct(4);
    Assert.IsType<NotFoundResult>(actual);
  }
    [Fact]
  public void DeleteProduct_GetProductById_NoContentResult()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(new Product { });
    mockRepo.Setup(x => x.DeleteProduct(It.IsAny<Product>()));
    var service = new ProductService(mockRepo.Object);

    var actual = service.DeleteProduct(4);
    Assert.IsType<NoContentResult>(actual);
  }
    [Fact]
  public void DeleteProduct_GetProductById_DatabaseUnavailableException()
  {
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).Throws(new InvalidOperationException());
    var service = new ProductService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.DeleteProduct(4));
  }
}
