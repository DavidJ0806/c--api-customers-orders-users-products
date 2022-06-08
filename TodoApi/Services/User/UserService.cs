using TodoApi.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Repositories;
using Serilog;
using TodoApi.Utilities.Exceptions;

namespace TodoApi.Services
{
    /// <summary>
    /// A service to use the dbcontext to interact with data
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// This method takes in parameters that are either null or provided. Returns list of users based on if the parameters match
        /// all of the properties for users
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="title">string</param>
        /// <param name="roles">string</param>
        /// <param name="email">string</param>
        /// <param name="password">string</param>
        /// <returns>Users</returns>
        public IEnumerable<User> GetAllUsersAsync(string name, string title, string roles, string email, string password)
        {
            try
            {
                return _userRepository.GetUsersByQuery(name, title, roles, email, password);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// This method finds a user by its id. Returns null if none found
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>User</returns>
        public ActionResult<User> GetUserById(int id)
        {
            try
            {
                var user = _userRepository.FindUserById(id);
                if (user == null) return new NotFoundResult();
                return new OkObjectResult(user);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// Uses regex to parse the given roles and return them as a json array in string form
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Boolean</returns>
        public ActionResult<User> PostUserAsync(User user)
        {
            bool emailTaken;
            try
            {
                emailTaken = _userRepository.CheckEmailsForPostUser(user.Email);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (emailTaken) return new ConflictResult();
            // SET ROLES TO A STRING ARRAY(?)
            Regex rgx = new Regex("^(EMPLOYEEADMIN|ADMINEMPLOYEE)$");
            user.Roles = rgx.IsMatch(user.Roles) ? "[EMPLOYEE, ADMIN]" : $"[{user.Roles}]";

            // CREATE USER
            try
            {
                return new OkObjectResult(_userRepository.PostUser(user));
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// Checks the database to see if the provided user to delete exists. Returns null otherwise
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>User</returns>
        public IActionResult DeleteUser(int id)
        {
            var user = new User();
            try
            {
                user = _userRepository.FindUserById(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (user == null) return new NotFoundResult();
            try
            {
                _userRepository.DeleteUserAsync(user);
                return new NoContentResult();
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// Finds the user by Id to see if it exists. Returns null if not found
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="id">Int</param>
        /// <returns>User</returns>
        public ActionResult<User> UpdateUserAsync(User user, int id)
        {
            // MISMATCHING IDS
            if (user.Id != id) return new BadRequestResult();
            User? userFromDb = new User();

            // REPO CALL
            try
            {
                userFromDb = _userRepository.FindUserById(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (userFromDb == null) return new NotFoundResult();

            User? userWithExistingEmail = new User();
            // IS EMAIL ALREADY TAKEN?
            if (user.Email != userFromDb.Email)
            {
                try
                {
                    userWithExistingEmail = _userRepository.UserWithExistingEmail(user.Email, id);
                    if (userWithExistingEmail != null) return new ConflictResult();
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error(ex.Message);
                    throw new DatabaseUnavailableException("Can't connect to the database");
                }
            }
            Regex rgx = new Regex("^(EMPLOYEEADMIN|ADMINEMPLOYEE)$");
            user.Roles = rgx.IsMatch(user.Roles) ? "[EMPLOYEE, ADMIN]" : $"[{user.Roles}]";
            // USER EXISTS
            try
            {
                _userRepository.UpdateUser(user);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            return new OkObjectResult(user);
        }
    }
}