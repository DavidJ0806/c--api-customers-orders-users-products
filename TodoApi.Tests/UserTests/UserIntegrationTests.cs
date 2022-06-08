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

public class UserControllerTests
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
  private async Task<UsersController> GetUserController()
  {
    var context = await GetDatabaseContext();
    var repository3 = new UserRepository(context);
    var userService = new UserService(repository3);
    var userController = new UsersController(userService);
    return userController;
  }
  [Fact]
  public void GetUsers_AllUsers_Returns200()
  {
    var controller = GetUserController().Result;

    var users = controller.GetUsers();
    var results = (OkObjectResult?)users.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void GetUser_ValidUser_Returns200()
  {
    var controller = GetUserController().Result;

    var users = controller.GetUserById(1);
    var results = (OkObjectResult?)users.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void GetUser_InvalidUser_Returns404()
  {
    var controller = GetUserController().Result;

    var users = controller.GetUserById(112);
    var results = (NotFoundResult?)users.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
  [Fact]
  public void PostUser_ValidUser_Returns200()
  {
    var controller = GetUserController().Result;

    var users = controller.PostUser(new User { Name = "Joe", Title = "Janitor", Roles = "[ADMIN]", Email = "j@2j.com", Password = "123pw" });
    var results = (OkObjectResult?)users.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }
  [Fact]
  public void PostUser_EmailTaken_Returns409()
  {
    var controller = GetUserController().Result;

    var users = controller.PostUser(new User { Name = "Joe", Title = "Janitor", Roles = "[ADMIN]", Email = "j@j.com", Password = "123pw" });
    var results = (ConflictResult?)users.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(409, statusCode);
  }

  [Fact]
  public void PutUser_EmailTaken_Returns409()
  {
    var controller = GetUserController().Result;

    var users = controller.PutUser(1, new User { Id = 1, Name = "David", Title = "Janitor", Roles = "[EMPLOYEE, ADMIN]", Email = "h@j.com", Password = "123pw" });
    var results = (ConflictResult?)users.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(409, statusCode);
  }

  [Fact]
  public void PutUser_UserDoesNotExist_Returns404()
  {
    var controller = GetUserController().Result;

    var users = controller.PutUser(100, new User { Id = 100, Name = "David", Title = "Janitor", Roles = "[EMPLOYEE, ADMIN]", Email = "h22j.com", Password = "123pw" });
    var results = (NotFoundResult?)users.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
  [Fact]
  public void PutUser_ValidUser_Returns200()
  {
    var controller = GetUserController().Result;

    var users = controller.PutUser(1, new User { Id = 1, Name = "David", Title = "Janitor", Roles = "[EMPLOYEE, ADMIN]", Email = "h22j.com", Password = "123pw" });
    var results = (OkObjectResult?)users.Result;
    var statusCode = results?.StatusCode;

    Assert.Equal(200, statusCode);
  }

  [Fact]
  public void DeleteUser_UserDoesNotExist_Returns404()
  {
    var controller = GetUserController().Result;

    var users = controller.DeleteUser(100);
    var results = (NotFoundResult?)users;
    var statusCode = results?.StatusCode;

    Assert.Equal(404, statusCode);
  }
   [Fact]
  public void DeleteUser_UserDoesExist_Returns204()
  {
    var controller = GetUserController().Result;

    var users = controller.DeleteUser(1);
    var results = (NoContentResult?)users;
    var statusCode = results?.StatusCode;

    Assert.Equal(204, statusCode);
  }

}