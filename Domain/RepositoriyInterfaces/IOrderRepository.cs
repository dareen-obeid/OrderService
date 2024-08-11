using System;
using Domain.Models;

namespace Domain.RepositoriyInterfaces
{
	public interface IOrderRepository
	{
        Task<Order> GetOrderById(int orderId);

        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId);
        Task<Order> CreateOrder(Order order);
        Task UpdateOrderAsync(Order order);
        Task CancelOrderAsync(int orderId);

        Task<OrderItem> CreateOrderItem(OrderItem orderItem);
        Task<OrderStatus> GetOrderStatusById(int orderStatusId);

    }
}

