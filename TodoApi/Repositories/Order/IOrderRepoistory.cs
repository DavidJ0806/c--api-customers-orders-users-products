using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Object> GetOrders(int? customerId = null, string? date = null, decimal? orderTotal = null, int? productId = null, int? quantity = null);
        Object? GetOrder(int id);
        Order CreateOrder(Order order);
        Order UpdateOrder(Order order);
        void DeleteOrder(Order order);
        Order? GetOrderToDelete(int id);
    }
}
