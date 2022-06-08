using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Contexts;
using Serilog;
namespace TodoApi.Controllers
{
    /// <summary>
    /// Users controller that exposes the endpoint to interact with user data
    /// </summary>
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Exposes the endpoint to retrieve users by using optional query parameters
        /// </summary>
        /// <param name="name">Optional string</param>
        /// <param name="title">Optional string</param>
        /// <param name="roles">Optional string</param>
        /// <param name="email">Optional string</param>
        /// <param name="password">Optional string</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers(string? name = null, string? title = null, string? roles = null, string? email = null, string? password = null)
        {
            Log.Information("Request received for get users by query");
            return Ok(_userService.GetAllUsersAsync(name, title, roles, email, password));
        }

        /// <summary>
        /// Exposes the endpoint to retrieve a user by its id
        /// </summary>
        /// <param name="id">long, path parameter</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            Log.Information("Request received for get user by id");
            return _userService.GetUserById(id);
        }

        /// <summary>
        /// Exposes the endpoint to update a user by id
        /// </summary>
        /// <param name="id">long, path parameter</param>
        /// <param name="user">User</param>
        /// <returns>User</returns>
        [HttpPut("{id}")]
        public ActionResult<User> PutUser(int id, User user)
        {
            Log.Information("Request recieved for update user");
            return _userService.UpdateUserAsync(user, id);
        }

        /// <summary>
        /// Exposes endpoint to create a new user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User</returns>
        [HttpPost]
        public ActionResult<User> PostUser(User user)
        {
            Log.Information("Request received for create user");
            return _userService.PostUserAsync(user);
        }

        /// <summary>
        /// Exposes the endpoint to delete a user by its id
        /// </summary>
        /// <param name="id">long, path parameter</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            Log.Information("Request received for create user");
            return _userService.DeleteUser(id);
        }
    }
}
