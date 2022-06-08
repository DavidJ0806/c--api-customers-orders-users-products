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

public class OrderControllerTests
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
  private async Task<OrdersController> GetOrderController()
  {
    var context = await GetDatabaseContext();
    var repository1 = new OrderRepository(context);
    var repository2 = new CustomerRepository(context);
    var repository3 = new ProductRepository(context);
    var orderService = new OrderService(repository1, repository2, repository3);
    var orderController = new OrdersController(orderService);
    return orderController;
  }

  [Fact]
  public void GetOrders_IntegrationTest_Returns200StatusCode()
  {
    var controller = GetOrderController().Result;
    
    var orders = controller.GetOrders();
    var results = (OkObjectResult?)orders.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void GetOrders_OrderIsNotInTheDatabase_Returns404StatusCode()
  {
    var controller = GetOrderController().Result;
    
    var orders = controller.GetOrder(100);
    var results = (NotFoundResult?)orders.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
    [Fact]
  public async void PutOrders_ValidOrder_Returns200StatusCode()
  {
    var controller = GetOrderController().Result;
    
    var orders = await controller.UpdateOrder(1, new Order { Id = 1, CustomerId = 1, Date = "1999-04-03", OrderTotal = 12.32m, OrderDetails = new OrderDetails { ProductId = 5, Quantity = 12 } });
    var results = (OkObjectResult?)orders.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
      [Fact]
  public async void PutOrders_OrderDoesNotExist_Returns404StatusCode()
  {
    var controller = GetOrderController().Result;
    
    var orders = await controller.UpdateOrder(100, new Order { Id = 100, CustomerId = 1, Date = "1999-04-03", OrderTotal = 12.32m, OrderDetails = new OrderDetails { ProductId = 5, Quantity = 12 } });
    var results = (NotFoundResult?)orders.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
      [Fact]
  public void PostOrders_ValidOrder_Returns201StatusCode()
  {
    var controller = GetOrderController().Result;
    
    var orders = controller.PostOrder(new Order { CustomerId = 1, Date = "1999-04-03", OrderTotal = 12.32m, OrderDetails = new OrderDetails { ProductId = 5, Quantity = 12 } });
    var results = (CreatedResult?)orders.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(201, statusCode);
  }
        [Fact]
  public void DeleteOrder_OrderExists_Returns204StatusCode()
  {
    var controller = GetOrderController().Result;

    var orders = controller.DeleteOrder(1);
    var results = (NoContentResult?)orders;
    var statusCode = results?.StatusCode;

    Assert.Equal(204, statusCode);
  }
     [Fact]
  public void DeleteOrder_OrderDoesNotExists_Returns404StatusCode()
  {
    var controller = GetOrderController().Result;

    var orders = controller.DeleteOrder(100);
    var results = (NotFoundResult?)orders;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
}