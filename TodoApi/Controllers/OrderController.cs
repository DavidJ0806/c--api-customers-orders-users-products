using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Contexts;
using Serilog;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Order controller to expose endpoints for interacting with Orders
    /// </summary>
    [Route("Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Exposes the endpoint to get orders using optional queries
        /// </summary>
        /// <param name="customerId">Optional int</param>
        /// <param name="date">Optional string</param>
        /// <param name="orderTotal">Optional decimal</param>
        /// <param name="productId">Optional int</param>
        /// <param name="quantity">Optional int</param>
        /// <returns>Orders</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders(int? customerId = null, string? date = null, decimal? orderTotal = null, int? productId = null, int? quantity = null)
        {
            Log.Information("Request received for get orders by query");
            return _orderService.GetOrders(customerId, date, orderTotal, productId, quantity);
        }

        /// <summary>
        /// Exposes the endpoint to get an order by Id
        /// </summary>
        /// <param name="id">Int, path parameter</param>
        /// <returns>Order</returns>
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            Log.Information("Request received for get order by id");
            return _orderService.GetOrder(id);
        }

        /// <summary>
        /// Exposes the endpoint for creating an order
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Created Order</returns>
        [HttpPost]
        public ActionResult<Order> PostOrder(Order order)
        {
            Log.Information("Request received for create order");
            return _orderService.CreateOrder(order);
        }

        /// <summary>
        /// Exposes the endpoint for updating orders by id
        /// </summary>
        /// <param name="id">Int, path parameter</param>
        /// <param name="order">Order</param>
        /// <returns>Updated Order</returns>
        [HttpPut("{id}")]
        public Task<ActionResult<Order>> UpdateOrder(int id, Order order)
        {
            Log.Information("Request received for update order");
            return Task.FromResult(_orderService.UpdateOrder(id, order));
        }
        /// <summary>
        /// Exposes the endpoint for deleting an order and remotes it from the database
        /// </summary>
        /// <param name="id">Int path param</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id)
        {
            Log.Information("Request received for delete order");
            return _orderService.DeleteOrder(id);
        }
    }
}