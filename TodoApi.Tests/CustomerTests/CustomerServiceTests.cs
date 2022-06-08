using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using TodoApi.Services;
using TodoApi.Repositories;
using Moq;
using TodoApi.Models;
using TodoApi.Utilities.Exceptions;
using System.Collections.Generic;

public class CustomerServiceTests
{
  [Fact]
  public void GetCustomerById_ProvideId1_Returns200()
  {
    var customer = new Customer
    {
      Id = 1,
      Name = "fred",
      Email = "fred@j.c",
      CustomerAddress = new CustomerAddress
      {
        Id = 1,
        Street = "street1",
        City = "city1",
        State = "state1",
        ZipCode = "99991"
      }
    };
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomerById(It.IsAny<int>())).Returns(customer);
    var service = new CustomerService(mock.Object);
    var actual = service.GetCustomerById(1);
    var results = (OkObjectResult?)actual.Result;
    var statusCode = results?.StatusCode;
    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void GetCustomerById_ProvideId1_Returns404()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomerById(It.IsAny<int>())).Throws(new InvalidOperationException("Sequence contains no elements"));
    var service = new CustomerService(mock.Object);
    var actual = service.GetCustomerById(14);
    Assert.IsType<NotFoundResult>(actual.Result);
  }
  [Fact]
  public void GetCustomerById_ProvidedId1_ThrowsDatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomerById(It.IsAny<int>())).Throws(new InvalidOperationException());
    var service = new CustomerService(mock.Object);
    Assert.Throws<DatabaseUnavailableException>(() => service.GetCustomerById(1));
  }
  [Fact]
  public void UpdateCustomer_UpdateFunctionality_ConflictResult()
  {
    var mock = new Mock<ICustomerRepository>();
    var service = new CustomerService(mock.Object);
    var actual = service.UpdateCustomer(1, new Customer { Id = 2 });
    Assert.IsType<ConflictResult>(actual.Result);
  }
  [Fact]
  public void UpdateCustomer_UpdateFunctionality_DatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(It.IsAny<int>())).Throws(new InvalidOperationException());
    var service = new CustomerService(mock.Object);
    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateCustomer(1, new Customer { Id = 1 }));
  }
  [Fact]
  public void UpdateCustomer_UpdateFunctionality_NotFoundResult()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(It.IsAny<int>())).Returns(value: null);
    var service = new CustomerService(mock.Object);
    var actual = service.UpdateCustomer(1, new Customer { Id = 1 });
    Assert.IsType<NotFoundResult>(actual.Result);
  }
  [Fact]
  public void UpdateCustomer_RepositoryEmailTaken_DatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(It.IsAny<int>())).Returns(new Customer { Id = 1 });
    mock.Setup(x => x.EmailTaken(It.IsAny<Customer>())).Throws(new InvalidOperationException());
    var service = new CustomerService(mock.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateCustomer(1, new Customer { Id = 1 }));
  }
  [Fact]
  public void UpdateCustomer_EmailTaken_ConflictResult()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(It.IsAny<int>())).Returns(new Customer { Id = 1 });
    mock.Setup(x => x.EmailTaken(It.IsAny<Customer>())).Returns(true);
    var service = new CustomerService(mock.Object);
    var actual = service.UpdateCustomer(1, new Customer { Id = 1 });
    Assert.IsType<ConflictResult>(actual.Result);
  }
  [Fact]
  public void UpdateCustomer_UpdateCustomer_OkObjectResultCustomer()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(It.IsAny<int>())).Returns(new Customer { Id = 1 });
    mock.Setup(x => x.EmailTaken(It.IsAny<Customer>())).Returns(false);
    mock.Setup(x => x.UpdateCustomer(It.IsAny<Customer>())).Returns(new Customer { Id = 1 });
    var service = new CustomerService(mock.Object);
    var actual = service.UpdateCustomer(1, new Customer { Id = 1 });
    Assert.IsType<OkObjectResult>(actual.Result);
  }
  [Fact]
  public void UpdateCustomer_UpdateCustomer_DatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(It.IsAny<int>())).Returns(new Customer { Id = 1 });
    mock.Setup(x => x.EmailTaken(It.IsAny<Customer>())).Returns(false);
    mock.Setup(x => x.UpdateCustomer(It.IsAny<Customer>())).Throws(new InvalidOperationException());
    var service = new CustomerService(mock.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateCustomer(1, new Customer { Id = 1 }));
  }

  [Fact]
  public void DeleteCustomer_GetCustomer_ThrowsDatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(1)).Throws(new InvalidOperationException());
    var service = new CustomerService(mock.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.DeleteCustomer(1));
  }

  [Fact]
  public void DeleteCustomer_GetCustomer_NotFoundResult()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(1)).Returns(value: null);
    var service = new CustomerService(mock.Object);

    var actual = service.DeleteCustomer(1);
    Assert.IsType<NotFoundResult>(actual);
  }

  [Fact]
  public void DeleteCustomer_DeleteCustomer_ThrowsDatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(1)).Returns(new Customer { Id = 1 });
    mock.Setup(x => x.DeleteCustomer(It.IsAny<Customer>())).Throws(new InvalidOperationException());
    var service = new CustomerService(mock.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.DeleteCustomer(1));
  }
  [Fact]
  public void DeleteCustomer_DeleteCustomer_NoContentResult()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomer(1)).Returns(new Customer { Id = 1 });
    mock.Setup(x => x.DeleteCustomer(It.IsAny<Customer>()));
    var service = new CustomerService(mock.Object);

    var actual = service.DeleteCustomer(1);
    Assert.IsType<NoContentResult>(actual);
  }

  [Fact]
  public void CreateCustomer_EmailTaken_ThrowsDatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.EmailTaken(It.IsAny<Customer>())).Throws(new InvalidOperationException());
    var service = new CustomerService(mock.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.CreateCustomer(new Customer { Id = 1 }));
  }
  [Fact]
  public void CreateCustomer_EmailTaken_ConflictResult()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.EmailTaken(It.IsAny<Customer>())).Returns(true);
    var service = new CustomerService(mock.Object);


    var actual = service.CreateCustomer(new Customer { Id = 1 });
    Assert.IsType<ConflictResult>(actual.Result);
  }

  [Fact]
  public void CreateCustomer_CreateCustomer_ThrowsDatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.EmailTaken(It.IsAny<Customer>())).Returns(false);
    mock.Setup(x => x.CreateCustomer(It.IsAny<Customer>())).Throws(new InvalidOperationException());

    var service = new CustomerService(mock.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.CreateCustomer(new Customer { Id = 1 }));
  }
  [Fact]
  public void CreateCustomer_CreateCustomer_CreatedResultCustomer()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.EmailTaken(It.IsAny<Customer>())).Returns(false);
    mock.Setup(x => x.CreateCustomer(It.IsAny<Customer>())).Returns(new Customer { Id = 1});

    var service = new CustomerService(mock.Object);

    var actual = service.CreateCustomer(new Customer { Id = 1 });
    Assert.IsType<CreatedResult>(actual.Result);
  }
  [Fact]
  public void GetCustomers_GetCustomers_ThrowsDatabaseUnavailableException()
  {
    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomers(null, null, null, null, null, null)).Throws(new InvalidOperationException());
    var service = new CustomerService(mock.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.GetCustomers(null, null, null, null, null, null));
  }
  [Fact]
  public void GetCustomers_GetCustomers_ReturnsCustomers()
  {
    List<Customer> customers = new List<Customer>();

    var mock = new Mock<ICustomerRepository>();
    mock.Setup(x => x.GetCustomers(null, null, null, null, null, null)).Returns(customers);
    var service = new CustomerService(mock.Object);

    var actual = service.GetCustomers(null, null, null, null, null, null);
    Assert.IsType<OkObjectResult>(actual.Result);
  }
}