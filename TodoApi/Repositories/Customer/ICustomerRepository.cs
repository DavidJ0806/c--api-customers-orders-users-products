using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ICustomerRepository
    {
        Customer? GetCustomer(int id);
        void DeleteCustomer(Customer customer);
        bool EmailTaken(Customer customer);
        Customer UpdateCustomer(Customer customer);
        Object GetCustomerById(int id);
        Customer CreateCustomer(Customer customer);
        IEnumerable<Object> GetCustomers(string name, string email, string street, string city, string state, string zipCode);
    }
}
