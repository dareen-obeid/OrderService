using System;
using Domain.Models;
using Domain.RepositoriyInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Order> GetOrderById(int orderId)
        {
             var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.IsActive);
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders
                    .Where(o => o.IsActive)
                    .Include(o => o.OrderItems)
                    .Include(o => o.OrderStatus)
                    .ToListAsync();
        }


        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId && o.IsActive)
                .Include(o => o.OrderItems)
                .Include(o => o.OrderStatus)
                .ToListAsync();
        }


        public async Task<Order> CreateOrder(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                item.CreatedDate = DateTime.UtcNow;
                item.LastUpdatedDate = DateTime.UtcNow;
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }


        public async Task UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            order.LastUpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await GetOrderById(orderId);
            if (order != null)
            {
                order.IsActive = false;
                order.LastUpdatedDate = DateTime.Now;
               await _context.SaveChangesAsync();
            }
        }



        public async Task<OrderItem> CreateOrderItem(OrderItem orderItem)
        {
            orderItem.CreatedDate = DateTime.UtcNow;
            orderItem.LastUpdatedDate = DateTime.UtcNow;
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderStatus> GetOrderStatusById(int orderStatusId)
        {
            return await _context.OrderStatuses.FindAsync(orderStatusId);
        }
    }
}

