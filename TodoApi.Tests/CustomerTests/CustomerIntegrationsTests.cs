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

public class CustomersControllerTests
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
  private async Task<CustomerService> GetCustomerService()
  {
    var context = await GetDatabaseContext();
    var repository = new CustomerRepository(context);
    var customerService = new CustomerService(repository);
    return customerService;
  }

  [Fact]
  public void GetCustomers_IntegrationTest_Returns200StatusCode()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customers = customerController.GetCustomers();
    var results = (OkObjectResult?)customers.Result;
    var statusCode = results?.StatusCode;

    // implicit in the CAST^ 
    // Assert.IsType<OkObjectResult>(results);
    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void GetCustomer_IntegrationTest_Returns200StatusCode()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customers = customerController.GetCustomer(1);
    var results = (OkObjectResult?)customers.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void GetCustomer_IntegrationTest_Returns404StatusCode()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customers = customerController.GetCustomer(100);
    var results = (NotFoundResult?)customers.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
  [Fact]
  public void PostCustomer_IntegrationTest_Returns201()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customer = new Customer { Name = "Test", Email = "Test", CustomerAddress = new CustomerAddress { Street = "street", City = "city", State = "state", ZipCode = "12345" } };
    var customers = customerController.PostCustomer(customer);
    var results = (CreatedResult?)customers.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(201, statusCode);
  }
  [Fact]
  public void PostCustomer_TestExistingEmail_Returns409()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customer = new Customer { Name = "Test", Email = "d@j.com1", CustomerAddress = new CustomerAddress { Street = "street", City = "city", State = "state", ZipCode = "12345" } };
    var customers = customerController.PostCustomer(customer);
    var results = (ConflictResult?)customers.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(409, statusCode);
  }
  [Fact]
  public void PutCustomer_ValidCustomer_Returns200()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customer = new Customer { Id = 1, Name = "Test", Email = "d@j.com1", CustomerAddress = new CustomerAddress { Street = "street", City = "city", State = "state", ZipCode = "12345" } };
    var customers = customerController.UpdateCustomer(1, customer);
    var results = (OkObjectResult?)customers.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void PutCustomer_InvalidCustomerId_Returns404()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customer = new Customer { Id = 100, Name = "Test", Email = "d@j.com1", CustomerAddress = new CustomerAddress { Street = "street", City = "city", State = "state", ZipCode = "12345" } };
    var customers = customerController.UpdateCustomer(100, customer);
    var results = (NotFoundResult?)customers.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }

  [Fact]
  public void PutCustomer_EmailAlreadyExists_Returns409()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customer = new Customer { Id = 1, Name = "Test", Email = "d@j.com3", CustomerAddress = new CustomerAddress { Street = "street", City = "city", State = "state", ZipCode = "12345" } };
    var customers = customerController.UpdateCustomer(1, customer);
    var results = (ConflictResult?)customers.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(409, statusCode);
  }
  [Fact]
  public void DeleteCustomer_CustomerToDelete_Returns204()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customers = customerController.DeleteCustomer(1);
    var results = (NoContentResult)customers;
    var statusCode = results?.StatusCode;

    Assert.Equal(204, statusCode);
  }
    [Fact]
  public void DeleteCustomer_CustomerNotInDatabase_Returns404()
  {
    var customerService = GetCustomerService();
    var customerController = new CustomersController(customerService.Result);

    var customers = customerController.DeleteCustomer(100);
    var results = (NotFoundResult?)customers;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
}