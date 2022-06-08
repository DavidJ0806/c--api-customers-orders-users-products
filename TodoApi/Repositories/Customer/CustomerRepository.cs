using TodoApi.Models;
using TodoApi.Contexts;
using TodoApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public readonly ApiDb db;
        public CustomerRepository(ApiDb context)
        {
            this.db = context;
        }
        public Customer? GetCustomer(int id)
        {
            return db.Customers.Find(id);
        }
        public void DeleteCustomer(Customer customer)
        {
            db.Customers.Remove(customer);
            db.SaveChanges();
        }
        public bool EmailTaken(Customer customer)
        {
            return db.Customers.Any(customerToCheck => (customer.Id != customerToCheck.Id) && (customerToCheck.Email == customer.Email));
        }
        public Customer UpdateCustomer(Customer customer)
        {
            var entry = db.Customers.First(o => o.Id == customer.Id);
            db.Entry(entry).CurrentValues.SetValues(customer);
            db.SaveChanges();
            return customer;
        }

        public Object GetCustomerById(int id)
        {
            var customer = db.Customers.LeftOuterJoin(
               db.CustomerAddresses,
               cust => cust.Id,
               add => add.CustomerId,
               (l, r) => new
               {
                   Id = l.Id,
                   Name = l.Name,
                   Email = l.Email,
                   CustomerAddress = new
                   {
                       Street = r?.Street,
                       City = r?.City,
                       State = r?.State,
                       ZipCode = r?.ZipCode
                   }
               }).Where(customer => customer.Id == id).First();
            return customer;
        }
        public Customer CreateCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
            return customer;
        }
        public IEnumerable<Object> GetCustomers(string name, string email, string street, string city, string state, string zipCode)
        {
            var contents = db.Customers.LeftOuterJoin(
              db.CustomerAddresses,
              cust => cust.Id,
              add => add.CustomerId,
              (l, r) => new
              {
                  Id = l.Id,
                  Name = l.Name,
                  Email = l.Email,
                  CustomerAddress = new
                  {
                      Street = r?.Street,
                      City = r?.City,
                      State = r?.State,
                      ZipCode = r?.ZipCode
                  }
              });
            var filteredCustomers = contents
            .Where(
              customer =>
              (name == null || customer.Name == name)
              && (email == null || customer.Email == email)
              && (street == null || customer.CustomerAddress.Street == street)
              && (city == null || customer.CustomerAddress.City == city)
              && (state == null || customer.CustomerAddress.State == state)
              && (zipCode == null || customer.CustomerAddress.ZipCode == zipCode)
              );
            return filteredCustomers;
        }
    }
}