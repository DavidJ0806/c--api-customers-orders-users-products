using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Repositories;
using Serilog;
using TodoApi.Controllers;
using TodoApi.Utilities.Exceptions;

namespace TodoApi.Services
{
    /// <summary>
    /// A service to use the dbcontext to interact with data
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// This method takes optional query parameters to filter orders
        /// </summary>
        /// <param name="customerId">int</param>
        /// <param name="date">string</param>
        /// <param name="orderTotal">decimal</param>
        /// <param name="productId">int</param>
        /// <param name="quantity">int</param>
        /// <returns>Ok result, IEnumerable Orders</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database is down</exception>
        public ActionResult<IEnumerable<Order>> GetOrders(int? customerId = null, string? date = null, decimal? orderTotal = null, int? productId = null, int? quantity = null)
        {
            try
            {
                return new OkObjectResult(_orderRepository.GetOrders(customerId, date, orderTotal, productId, quantity));
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// This method handles the repository call to get an order by its id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Ok result, order</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database is down</exception>
        public ActionResult<Order> GetOrder(int id)
        {
            Object order = new Order();
            try
            {
                order = _orderRepository.GetOrder(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (order == null) return new NotFoundResult();
            return new OkObjectResult(order);
        }

        /// <summary>
        /// Handles the logic for creating customers and calls repository
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Created result, Order</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database is down</exception>
        public ActionResult<Order> CreateOrder(Order order)
        {
            Customer? customer = new Customer();

            // DOES CUSTOMER EXIST
            try
            {
                customer = _customerRepository.GetCustomer(Convert.ToInt16(order.CustomerId));
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (customer == null) return new BadRequestResult();

            Product? product = new Product();
            // DOES PRODUCT EXIST
            try
            {
                product = _productRepository.GetProductById(order.OrderDetails.ProductId);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (product == null) return new BadRequestResult();
            Order createdOrder = new Order();
            order.OrderDetails.OrderId = Convert.ToInt32(order.Id);
            // CREATE ORDER
            try
            {
                createdOrder = _orderRepository.CreateOrder(order);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            return new CreatedResult(nameof(OrdersController), createdOrder);
        }

        /// <summary>
        /// This method calls the repository to update orders
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="order">Order</param>
        /// <returns>Ok result, Order</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database is down</exception>
        public ActionResult<Order> UpdateOrder(int id, Order order)
        {
            if (id != order.Id) return new BadRequestResult();
            Customer? customer = new Customer();
            Object orderToUpdate = new Object();
            try
            {
                orderToUpdate = _orderRepository.GetOrder(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (orderToUpdate == null) return new NotFoundResult();
            try
            {
                customer = _customerRepository.GetCustomer(Convert.ToInt16(order.CustomerId));
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (customer == null) return new BadRequestResult();
            Product? product = new Product();
            try
            {
                product = _productRepository.GetProductById(order.OrderDetails.ProductId);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (product == null) return new BadRequestResult();
            order.OrderDetails.OrderId = Convert.ToInt32(order.Id);
            try
            {
                return new OkObjectResult(_orderRepository.UpdateOrder(order));
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
        }

        /// <summary>
        /// This method calls the repository to make delete requests for orders
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>No content</returns>
        /// <exception cref="DatabaseUnavailableException">Thrown when database is down</exception>
        public ActionResult DeleteOrder(int id)
        {
            Order? order = new Order();
            try
            {
                order = _orderRepository.GetOrderToDelete(id);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            if (order == null) return new NotFoundResult();

            try
            {
                _orderRepository.DeleteOrder(order);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                throw new DatabaseUnavailableException("Can't connect to the database");
            }
            return new NoContentResult();
        }
    }
}