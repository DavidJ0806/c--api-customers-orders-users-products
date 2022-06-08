using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Contexts
{
    public static class Extensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
              new User { Id = 1, Name = "David", Title = "Janitor", Roles = "[EMPLOYEE, ADMIN]", Email = "d@j.com", Password = "123pw" },
              new User { Id = 2, Name = "Amir", Title = "Cleaner", Roles = "[EMPLOYEE]", Email = "A@j.com", Password = "123pw" },
              new User { Id = 3, Name = "Hayes", Title = "Boss", Roles = "[EMPLOYEE, ADMIN]", Email = "h@j.com", Password = "123pw" },
              new User { Id = 4, Name = "Cody", Title = "HR", Roles = "[ADMIN]", Email = "c@j.com", Password = "123pw" },
              new User { Id = 5, Name = "Joe", Title = "Janitor", Roles = "[ADMIN]", Email = "j@j.com", Password = "123pw" }
             );
            modelBuilder.Entity<Customer>().HasData(
              new Customer { Id = 1, Name = "Customer1", Email = "d@j.com1" },
              new Customer { Id = 2, Name = "Customer2", Email = "d@j.com2" },
              new Customer { Id = 3, Name = "Customer3", Email = "d@j.com3" }
              );
            modelBuilder.Entity<CustomerAddress>().HasData(
              new CustomerAddress { Id = 1, Street = "Street1", City = "City1", ZipCode = "22341", State = "CA", CustomerId = 1 },
              new CustomerAddress { Id = 2, Street = "Street2", City = "City2", ZipCode = "22342", State = "CA", CustomerId = 2 }
              );
            modelBuilder.Entity<Product>().HasData(
              new Product { Id = 1, Sku = "23451", Type = "Hello1", Name = "name1", Description = "descript1", Manufacturer = "homedepot1", Price = 42.15m },
              new Product { Id = 2, Sku = "23452", Type = "Hello2", Name = "name2", Description = "descript2", Manufacturer = "homedepot2", Price = 42.25m },
              new Product { Id = 3, Sku = "23453", Type = "Hello3", Name = "name3", Description = "descript3", Manufacturer = "homedepot3", Price = 42.35m },
              new Product { Id = 9, Sku = "29459", Type = "Hello9", Name = "name9", Description = "descript9", Manufacturer = "homedepot9", Price = 42.95m },
              new Product { Id = 4, Sku = "23454", Type = "Hello4", Name = "name4", Description = "descript4", Manufacturer = "homedepot4", Price = 42.45m },
              new Product { Id = 5, Sku = "23455", Type = "Hello5", Name = "name5", Description = "descript5", Manufacturer = "homedepot5", Price = 42.55m },
              new Product { Id = 6, Sku = "23456", Type = "Hello6", Name = "name6", Description = "descript6", Manufacturer = "homedepot6", Price = 42.65m },
              new Product { Id = 7, Sku = "23457", Type = "Hello7", Name = "name7", Description = "descript7", Manufacturer = "homedepot7", Price = 42.75m },
              new Product { Id = 8, Sku = "23458", Type = "Hello8", Name = "name8", Description = "descript8", Manufacturer = "homedepot8", Price = 42.85m }
            );
            modelBuilder.Entity<Order>().HasData(
              new Order { Id = 1, CustomerId = 1, Date = "1999-04-03", OrderTotal = 12.32m },
              new Order { Id = 2, CustomerId = 2, Date = "1999-04-03", OrderTotal = 12.32m },
              new Order { Id = 3, CustomerId = 3, Date = "1999-04-03", OrderTotal = 12.32m }
            );
            modelBuilder.Entity<OrderDetails>().HasData(
              new OrderDetails { Id = 1, OrderId = 1, ProductId = 5, Quantity = 12 },
              new OrderDetails { Id = 2, OrderId = 2, ProductId = 6, Quantity = 5 },
              new OrderDetails { Id = 3, OrderId = 3, ProductId = 2, Quantity = 22 }
            );
        }
    }
}