using TodoApi.Models;
using TodoApi.Contexts;
using TodoApi.Controllers;

namespace TodoApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public readonly ApiDb db;
        public OrderRepository(ApiDb context)
        {
            this.db = context;
        }
        public IEnumerable<Object> GetOrders(int? customerId = null, string? date = null, decimal? orderTotal = null, int? productId = null, int? quantity = null)
        {
            var orders = db.Orders.LeftOuterJoin(
              db.OrderDetails,
              order => order.Id,
              details => details.OrderId,
              (l, r) => new
              {
                  Id = l.Id,
                  customerId = l.CustomerId,
                  date = l.Date,
                  orderTotal = l.OrderTotal,
                  orderDetails = new
                  {
                      productId = r?.ProductId,
                      quantity = r?.Quantity
                  }
              }
            );
            var filteredOrders = orders.Where(
              order => (customerId == null || customerId == order.customerId) && (date == null || date == order.date)
              && (orderTotal == null || order.orderTotal == orderTotal) && (productId == null || productId == order.orderDetails.productId)
              && (quantity == null || quantity == order.orderDetails.quantity)
              );
            return filteredOrders;
        }
        public Object? GetOrder(int id)
        {

            var order = db.Orders.LeftOuterJoin(
                db.OrderDetails,
                order => order.Id,
                details => details.OrderId,
        (l, r) => new
        {
            Id = l.Id,
            customerId = l.CustomerId,
            date = l.Date,
            orderTotal = l.OrderTotal,
            orderDetails = new
            {
                productId = r?.ProductId,
                quantity = r?.Quantity
            }
        }
      ).FirstOrDefault(x => x.Id == id);
            return order;
        }
        public Order CreateOrder(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order;
        }
        public Order? GetOrderToDelete(int id)
        {
            return db.Orders.Find(id);
        }

        public Order UpdateOrder(Order order)
        {
            var entry = db.Orders.First(o => o.Id == order.Id);
            db.Entry(entry).CurrentValues.SetValues(order);
            db.SaveChanges();
            return order;
        }
        public void DeleteOrder(Order order)
        {
            db.Orders.Remove(order);
            db.SaveChanges();
        }
    }
}