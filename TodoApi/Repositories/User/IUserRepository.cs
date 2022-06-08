using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IUserRepository
    {
        void DeleteUserAsync(User user);
        User? FindUserById(int id);
        User UpdateUser(User user);
        User PostUser(User user);
        User? UserWithExistingEmail(string email, int id);
        bool CheckEmailsForPostUser(string email);
        IEnumerable<User> GetUsersByQuery(string name, string title, string roles, string email, string password);
    }

}