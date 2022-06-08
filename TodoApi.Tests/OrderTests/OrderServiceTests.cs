using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using TodoApi.Services;
using TodoApi.Repositories;
using Moq;
using TodoApi.Models;
using TodoApi.Utilities.Exceptions;
using System.Collections.Generic;

public class OrderServiceTests
{
  [Fact]
  public void GetOrders_GetOrders_ReturnsOkResult()
  {
    List<Order> orders = new List<Order>();

    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrders(null, null, null, null, null)).Returns(orders);
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.GetOrders();
    Assert.IsType<OkObjectResult>(actual.Result);
  }
  [Fact]
  public void GetOrders_GetOrders_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrders(null, null, null, null, null)).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.GetOrders());
  }
  [Fact]
  public void GetOrders_GetOrder_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrder(2)).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.GetOrder(2));
  }
  [Fact]
  public void GetOrders_GetOrder_ReturnsOkResult()
  {
    Order order = new Order();

    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrder(1)).Returns(new OkObjectResult(order));
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.GetOrder(1);
    Assert.IsType<OkObjectResult>(actual.Result);
  }
  [Fact]
  public void CreateOrder_GetCustomer_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    customerRepo.Setup(x => x.GetCustomer(1)).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.CreateOrder(new Order { CustomerId = 1 }));
  }
  [Fact]
  public void CreateOrder_GetCustomer_ReturnsBadRequestResult()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    customerRepo.Setup(x => x.GetCustomer(1)).Returns(value: null);
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.CreateOrder(new Order { CustomerId = 1 });
    Assert.IsType<BadRequestResult>(actual.Result);
  }
  [Fact]
  public void CreateOrder_GetProductById_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    customerRepo.Setup(x => x.GetCustomer(1)).Returns(new Customer { Id = 1 });
    productRepo.Setup(x => x.GetProductById(1)).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.CreateOrder(new Order { CustomerId = 1, OrderDetails = new OrderDetails { ProductId = 1 } }));
  }
  [Fact]
  public void CreateOrder_GetProductById_BadRequestResult()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    customerRepo.Setup(x => x.GetCustomer(1)).Returns(new Customer { Id = 1 });
    productRepo.Setup(x => x.GetProductById(1)).Returns(value: null);
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.CreateOrder(new Order { CustomerId = 1, OrderDetails = new OrderDetails { ProductId = 1 } });
    Assert.IsType<BadRequestResult>(actual.Result);
  }
  [Fact]
  public void CreateOrder_CreateOrder_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    customerRepo.Setup(x => x.GetCustomer(1)).Returns(new Customer { Id = 1 });
    productRepo.Setup(x => x.GetProductById(1)).Returns(new Product { Id = 1 });
    orderRepo.Setup(x => x.CreateOrder(It.IsAny<Order>())).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.CreateOrder(new Order { CustomerId = 1, OrderDetails = new OrderDetails { ProductId = 1 } }));

  }
  [Fact]
  public void CreateOrder_CreateOrder_ReturnsCreatedResult()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    customerRepo.Setup(x => x.GetCustomer(1)).Returns(new Customer { Id = 1 });
    productRepo.Setup(x => x.GetProductById(1)).Returns(new Product { Id = 1 });
    orderRepo.Setup(x => x.CreateOrder(It.IsAny<Order>())).Returns(new Order { Id = 1 });
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.CreateOrder(new Order { CustomerId = 1, OrderDetails = new OrderDetails { ProductId = 1 } });
    Assert.IsType<CreatedResult>(actual.Result);
  }
  [Fact]
  public void UpdateOrder_MismatchingIds_ReturnsBadRequest()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.UpdateOrder(3, new Order { Id = 2 });
    Assert.IsType<BadRequestResult>(actual.Result);
  }
  [Fact]
  public void UpdateOrder_GetCustomer_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrder(2)).Returns(new Order());
    customerRepo.Setup(x => x.GetCustomer(1)).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateOrder(2, new Order { Id = 2, CustomerId = 1 }));
  }
  [Fact]
  public void UpdateOrder_CustomerDoesntExist_ReturnsBadRequest()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrder(2)).Returns(new Order());
    customerRepo.Setup(x => x.GetCustomer(4)).Returns(value: null);
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.UpdateOrder(2, new Order { Id = 2, CustomerId = 4 });
    Assert.IsType<BadRequestResult>(actual.Result);
  }
  [Fact]
  public void UpdateOrder_GetProductById_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    customerRepo.Setup(x => x.GetCustomer(4)).Returns(new Customer { Id = 4 });
    orderRepo.Setup(x => x.GetOrder(2)).Returns(new Order());
    productRepo.Setup(x => x.GetProductById(4)).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateOrder(2, new Order { Id = 2, CustomerId = 4, OrderDetails = new OrderDetails { ProductId = 4 } }));
  }
  [Fact]
  public void UpdateOrder_GetProductById_ReturnsBadRequestResult()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    customerRepo.Setup(x => x.GetCustomer(4)).Returns(new Customer { Id = 4 });
    orderRepo.Setup(x => x.GetOrder(2)).Returns(new Order());
    productRepo.Setup(x => x.GetProductById(4)).Returns(value: null);
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.UpdateOrder(2, new Order { Id = 2, CustomerId = 4, OrderDetails = new OrderDetails { ProductId = 4 } });
    Assert.IsType<BadRequestResult>(actual.Result);
  }
  [Fact]
  public void UpdateOrder_GetOrder_ReturnsNotFoundResult()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrder(2)).Returns(value: null);
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.UpdateOrder(2, new Order { Id = 2, CustomerId = 4, OrderDetails = new OrderDetails { ProductId = 4 } });
    Assert.IsType<NotFoundResult>(actual.Result);
  }
  [Fact]
  public void UpdateOrder_UpdateOrder_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrder(2)).Returns(new Order());
    customerRepo.Setup(x => x.GetCustomer(4)).Returns(new Customer { Id = 4 });
    productRepo.Setup(x => x.GetProductById(4)).Returns(new Product { Id = 4 });
    orderRepo.Setup(x => x.UpdateOrder(It.IsAny<Order>())).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateOrder(2, new Order { Id = 2, CustomerId = 4, OrderDetails = new OrderDetails { ProductId = 4 } }));
  }

  [Fact]
  public void UpdateOrder_OrderRepoGetOrder_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrder(2)).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateOrder(2, new Order { Id = 2, CustomerId = 4, OrderDetails = new OrderDetails { ProductId = 4 } }));
  }
  [Fact]
  public void UpdateOrder_UpdateOrder_ReturnsOkObjectResult()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrder(2)).Returns(new Order());
    customerRepo.Setup(x => x.GetCustomer(4)).Returns(new Customer { Id = 4 });
    productRepo.Setup(x => x.GetProductById(4)).Returns(new Product { Id = 4 });
    orderRepo.Setup(x => x.UpdateOrder(It.IsAny<Order>())).Returns(new Order { Id = 1 });
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.UpdateOrder(2, new Order { Id = 2, CustomerId = 4, OrderDetails = new OrderDetails { ProductId = 4 } });
    Assert.IsType<OkObjectResult>(actual.Result);
  }
  [Fact]
  public void DeleteOrder_GetOrderToDelete_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrderToDelete(4)).Throws(new InvalidOperationException());
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.DeleteOrder(4));
  }
  [Fact]
  public void DeleteOrder_GetOrderToDelete_ReturnsNotFoundResult()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrderToDelete(4)).Returns(value: null);
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.DeleteOrder(4);
    Assert.IsType<NotFoundResult>(actual);
  }
  [Fact]
  public void DeleteOrder_DeleteOrder_ReturnsNoContent()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrderToDelete(4)).Returns(new Order { Id = 1 });
    orderRepo.Setup(x => x.DeleteOrder(It.IsAny<Order>()));
    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    var actual = service.DeleteOrder(4);
    Assert.IsType<NoContentResult>(actual);
  }
  [Fact]
  public void DeleteOrder_DeleteOrder_ThrowsDatabaseUnavailableException()
  {
    var productRepo = new Mock<IProductRepository>();
    var customerRepo = new Mock<ICustomerRepository>();
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(x => x.GetOrderToDelete(4)).Returns(new Order { Id = 1 });
    orderRepo.Setup(x => x.DeleteOrder(It.IsAny<Order>())).Throws(new InvalidOperationException());

    var service = new OrderService(orderRepo.Object, customerRepo.Object, productRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.DeleteOrder(4));
  }
}