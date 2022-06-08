using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using TodoApi.Services;
using TodoApi.Repositories;
using Moq;
using TodoApi.Models;
using TodoApi.Utilities.Exceptions;
using System.Collections.Generic;

public class UserServiceTests
{
  [Fact]
  public void GetAllUsersAsync_CallRepoGetUsersByQuery_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo
      .Setup(x => x.GetUsersByQuery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
      .Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.GetAllUsersAsync("1", "2", "3", "4", "5"));
  }
  [Fact]
  public void GetAllUsersAsync_CallRepoGetUsersByQuery_IsList()
  {
    List<User> users = new List<User> { };
    var mockRepo = new Mock<IUserRepository>();
    mockRepo
      .Setup(x => x.GetUsersByQuery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
      .Returns(users);
    var service = new UserService(mockRepo.Object);

    var actual = service.GetAllUsersAsync("1", "2", "3", "4", "5");
    Assert.Equal(users, actual);
  }
  [Fact]
  public void GetUserById_FindUserById_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.GetUserById(4));

  }
  [Fact]
  public void GetUserById_FindUserById_ReturnsNotFoundResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(value: null);
    var service = new UserService(mockRepo.Object);

    var actual = service.GetUserById(4);
    Assert.IsType<NotFoundResult>(actual.Result);
  }
  [Fact]
  public void GetUserById_FindUserById_ReturnsOkObjectResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(new User { });
    var service = new UserService(mockRepo.Object);

    var actual = service.GetUserById(4);
    Assert.IsType<OkObjectResult>(actual.Result);
  }
  [Fact]
  public void PostUserAsync_CheckEmailsForPostUser_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.CheckEmailsForPostUser(It.IsAny<string>())).Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.PostUserAsync(new User { }));
  }
  [Fact]
  public void PostUserAsync_CheckEmailsForPostUser_ReturnsConflictResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.CheckEmailsForPostUser(It.IsAny<string>())).Returns(true);
    var service = new UserService(mockRepo.Object);

    var actual = service.PostUserAsync(new User { });
    Assert.IsType<ConflictResult>(actual.Result);
  }
  [Fact]
  public void PostUserAsync_PostUser_ReturnsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.CheckEmailsForPostUser(It.IsAny<string>())).Returns(false);
    mockRepo.Setup(x => x.PostUser(It.IsAny<User>())).Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.PostUserAsync(new User { Roles = "EMPLOYEE" }));
  }

  [Fact]
  public void PostUserAsync_PostUser_ReturnsOkObjectResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.CheckEmailsForPostUser(It.IsAny<string>())).Returns(false);
    mockRepo.Setup(x => x.PostUser(It.IsAny<User>())).Returns(new User { Roles = "EMPLOYEE" });
    var service = new UserService(mockRepo.Object);

    var actual = service.PostUserAsync(new User { Roles = "EMPLOYEEADMIN" });
    Assert.IsType<OkObjectResult>(actual.Result);
  }
  [Fact]
  public void DeleteUser_FindUserById_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.DeleteUser(4));
  }
  [Fact]
  public void DeleteUser_FindUserById_ReturnsNotFoundResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(value: null);
    var service = new UserService(mockRepo.Object);

    var actual = service.DeleteUser(4);
    Assert.IsType<NotFoundResult>(actual);

  }
  [Fact]
  public void DeleteUser_DeleteUserAsync_ReturnsNoContentResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(new User { });
    mockRepo.Setup(x => x.DeleteUserAsync(It.IsAny<User>()));
    var service = new UserService(mockRepo.Object);

    var actual = service.DeleteUser(4);
    Assert.IsType<NoContentResult>(actual);

  }
  [Fact]
  public void DeleteUser_DeleteUserAsync_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(new User { });
    mockRepo.Setup(x => x.DeleteUserAsync(It.IsAny<User>())).Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.DeleteUser(4));
  }
  [Fact]
  public void UpdateUserAsync_MismatchingIds_ReturnsBadRequestResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    var service = new UserService(mockRepo.Object);

    var actual = service.UpdateUserAsync(new User { Id = 4 }, 3);
    Assert.IsType<BadRequestResult>(actual.Result);
  }
  [Fact]
  public void UpdateUserAsync_FindUserById_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateUserAsync(new User { Id = 4 }, 4));

  }
  [Fact]
  public void UpdateUserAsync_FindUserById_ReturnsNotFoundResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(value: null);
    var service = new UserService(mockRepo.Object);

    var actual = service.UpdateUserAsync(new User { Id = 4 }, 4);
    Assert.IsType<NotFoundResult>(actual.Result);
  }
  [Fact]
  public void UpdateUserAsync_UserWithExistingEmail_ReturnsConflictResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(new User { Email= "4" });
    mockRepo.Setup(x => x.UserWithExistingEmail(It.IsAny<string>(), It.IsAny<int>())).Returns(new User { Id = 5 });
    var service = new UserService(mockRepo.Object);

    var actual = service.UpdateUserAsync(new User { Id = 4, Email = "3" }, 4);
    Assert.IsType<ConflictResult>(actual.Result);
  }
  [Fact]
  public void UpdateUserAsync_UserWithExistingEmail_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(new User { Email = "3" });
    mockRepo.Setup(x => x.UserWithExistingEmail(It.IsAny<string>(), It.IsAny<int>())).Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateUserAsync(new User { Id = 4, Email = "2" }, 4));
  }
  [Fact]
    public void UpdateUserAsync_UpdateUser_ThrowsDatabaseUnavailableException()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(new User { });
    mockRepo.Setup(x => x.UserWithExistingEmail(It.IsAny<string>(), It.IsAny<int>())).Returns(new User { });
    mockRepo.Setup(x => x.UpdateUser(It.IsAny<User>())).Throws(new InvalidOperationException());
    var service = new UserService(mockRepo.Object);

    Assert.Throws<DatabaseUnavailableException>(() => service.UpdateUserAsync(new User { Id = 4, Roles = "EMPLOYEE" }, 4));
  }
  [Fact]
   public void UpdateUserAsync_UpdateUser_ReturnsOkObjectResult()
  {
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(x => x.FindUserById(It.IsAny<int>())).Returns(new User { Email = "2"});
    mockRepo.Setup(x => x.UserWithExistingEmail(It.IsAny<string>(), It.IsAny<int>())).Returns(value: null);
    mockRepo.Setup(x => x.UpdateUser(It.IsAny<User>())).Returns(new User{});
    var service = new UserService(mockRepo.Object);

    var actual = service.UpdateUserAsync(new User { Id = 4, Roles = "EMPLOYEEADMIN", Email = "1" }, 4);
    Assert.IsType<OkObjectResult>(actual.Result);
  }
}