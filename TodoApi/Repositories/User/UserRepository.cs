using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Contexts;

namespace TodoApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly ApiDb db;
        public UserRepository(ApiDb context)
        {
            this.db = context;
        }
        public async void DeleteUserAsync(User user)
        {
            db.Users.Remove(user);
            await db.SaveChangesAsync();
        }
        public User? FindUserById(int id)
        {
            return db.Users.Find(id);
        }
        public User UpdateUser(User user)
        {
            var entry = db.Users.First(u => u.Id == user.Id);
            db.Entry(entry).CurrentValues.SetValues(user);
            db.SaveChanges();
            return user;
        }
        public User PostUser(User user)
        {
            db.Users.Add(user);
            db.SaveChangesAsync();
            return user;
        }
        public User? UserWithExistingEmail(string email, int id)
        {
            return db.Users.Where(userToCheck => (id != userToCheck.Id) && (userToCheck.Email == email)).FirstOrDefault();
        }
        public bool CheckEmailsForPostUser(string email)
        {
            return db.Users.Any(user => user.Email == email);
        }
        public IEnumerable<User> GetUsersByQuery(string name, string title, string roles, string email, string password)
        {
            return db.Users
              .Where(user =>
              ((roles == null || user.Roles.Contains(roles) || user.Roles == "[EMPLOYEE, ADMIN]" && roles.Length == 13)
              && (name == null || user.Name == name) && (title == null || user.Title == title)
              && (email == null || user.Email == email) && (password == null || user.Password == password)));
        }
    }
}