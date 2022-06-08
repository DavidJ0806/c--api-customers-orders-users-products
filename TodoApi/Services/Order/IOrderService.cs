using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Services
{
    public interface IOrderService
    {
        ActionResult<IEnumerable<Order>> GetOrders(int? customerId = null, string? date = null, decimal? orderTotal = null, int? productId = null, int? quantity = null);
        ActionResult<Order> GetOrder(int id);
        ActionResult<Order> CreateOrder(Order order);
        ActionResult<Order> UpdateOrder(int id, Order order);
        ActionResult DeleteOrder(int id);
    }
}