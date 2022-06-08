using TodoApi.Models;
using TodoApi.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Repositories;
using Serilog;
using TodoApi.Controllers;

namespace TodoApi.Services
{
    /// <summary>
    /// A service to use the dbcontext to interact with data
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// This method calls the repository to update customer information
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="customer">Customer</param>
        /// <returns>Ok result, Customer</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when db is down</exception>
        public ActionResult<Customer> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.Id) return new ConflictResult();
            Customer? customerToUpdate = new Customer();
            try
            {
                customerToUpdate = _customerRepository.GetCustomer(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (customerToUpdate == null) return new NotFoundResult();
            bool emailTaken;
            try
            {
                emailTaken = _customerRepository.EmailTaken(customer);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (emailTaken) return new ConflictResult();
            try
            {
                return new OkObjectResult(_customerRepository.UpdateCustomer(customer));
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// This method calls the repository layer to delete customers by id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>No content</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when db is down</exception>
        public ActionResult DeleteCustomer(int id)
        {
            Customer? customer = new Customer();
            // GET CUSTOMER 
            try
            {
                customer = _customerRepository.GetCustomer(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (customer == null) return new NotFoundResult();

            // DELETE CUSTOMER
            try
            {
                _customerRepository.DeleteCustomer(customer);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            return new NoContentResult();
        }

        /// <summary>
        /// This method calls the repository to retrieve a customer by its id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Ok result, Customer</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when db is down</exception>
        public ActionResult<Customer> GetCustomerById(int id)
        {
            Object? customer = new Customer();
            try
            {
                customer = _customerRepository.GetCustomerById(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                if (ex.Message == "Sequence contains no elements") return new NotFoundResult();
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            return new OkObjectResult(customer);
        }

        /// <summary>
        /// This method calls the repository to post a new customer to the database
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Created result, Customer</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database is down</exception>
        public ActionResult<Customer> CreateCustomer(Customer customer)
        {
            bool emailTaken;
            try
            {
                emailTaken = _customerRepository.EmailTaken(customer);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (emailTaken) return new ConflictResult();

            try
            {
                return new CreatedResult(nameof(CustomersController), _customerRepository.CreateCustomer(customer));
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// This method calls the repository to retrieve customers based on optional query parameters
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="email">string</param>
        /// <param name="street">string</param>
        /// <param name="city">string</param>
        /// <param name="state">string</param>
        /// <param name="zipCode">string</param>
        /// <returns>Ok Result, IEnumerable Customers</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database is down</exception>
        public ActionResult<IEnumerable<Customer>> GetCustomers(string name, string email, string street, string city, string state, string zipCode)
        {
            try
            {
                return new OkObjectResult(_customerRepository.GetCustomers(name, email, street, city, state, zipCode));
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }
    }
}