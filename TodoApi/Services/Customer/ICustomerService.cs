using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Services
{
    public interface ICustomerService
    {
        ActionResult<Customer> UpdateCustomer(int id, Customer customer);
        ActionResult DeleteCustomer(int id);
        ActionResult<Customer> GetCustomerById(int id);
        ActionResult<Customer> CreateCustomer(Customer customer);
        ActionResult<IEnumerable<Customer>> GetCustomers(string name, string email, string street, string city, string state, string zipCode);
    }
}