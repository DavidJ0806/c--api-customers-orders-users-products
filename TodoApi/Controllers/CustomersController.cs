using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using Serilog;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Customers Controller that exposes endpoints for interacting with Customer data
    /// </summary>
    [Route("Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Exposes the endpoint to delete a customer by id
        /// </summary>
        /// <param name="id">Int</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            Log.Information("Request Received for delete customer");
            return _customerService.DeleteCustomer(id);
        }

        /// <summary>
        /// Exposes the endpoint to update a customer by its id
        /// </summary>
        /// <param name="id">Int, path parameter</param>
        /// <param name="customer">Customer</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<Customer> UpdateCustomer(int id, Customer customer)
        {
            Log.Information("Request received for Update customer");
            return _customerService.UpdateCustomer(id, customer);
        }

        /// <summary>
        /// Exposes the endpoint to retrieve a customer by its id
        /// </summary>
        /// <param name="id">Int, path parameter</param>
        /// <returns>Customer</returns>
        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            Log.Information("Request received for get customer by id");
            return _customerService.GetCustomerById(id);
        }

        /// <summary>
        /// Exposes the endpoint to create a new customer
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Customer</returns>
        [HttpPost]
        public ActionResult<Customer> PostCustomer(Customer customer)
        {
            Log.Information("Request received for create customer");
            return _customerService.CreateCustomer(customer);
        }

        /// <summary>
        /// Exposes the endpoint to get customers using optional query parameters
        /// </summary>
        /// <param name="name">Optional string</param>
        /// <param name="email">Optional string</param>
        /// <param name="street">Optional string</param>
        /// <param name="city">Optional string</param>
        /// <param name="state">Optional string</param>
        /// <param name="zipCode">Optional string</param>
        /// <returns>Customers</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetCustomers(string? name = null, string? email = null, string? street = null, string? city = null, string? state = null, string? zipCode = null)
        {
            Log.Information("Request received for get customers by query");
            return _customerService.GetCustomers(name, email, street, city, state, zipCode);
        }
    }
}