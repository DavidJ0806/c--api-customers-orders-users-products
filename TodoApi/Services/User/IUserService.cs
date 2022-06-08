using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsersAsync(string name, string title, string roles, string email, string password);
        ActionResult<User> GetUserById(int id);
        ActionResult<User> PostUserAsync(User user);
        IActionResult DeleteUser(int id);
        ActionResult<User> UpdateUserAsync(User user, int id);
    }
}