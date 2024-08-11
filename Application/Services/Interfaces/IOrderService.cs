using System;
using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderById(int orderId);
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(int customerId);
        Task UpdateOrder(OrderDto orderDto);
        Task<OrderDto> CreateOrder(OrderDto orderDto);
        Task CancelOrder(int orderId);
    }
}

