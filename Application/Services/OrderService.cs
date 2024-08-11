using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.Services.Interfaces;
using Application.Validation;
using AutoMapper;
using Domain.Models;
using Domain.RepositoriyInterfaces;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OrderDto> _orderValidator;
        private readonly IValidator<OrderItemDto> _orderItemValidator;
        private readonly IValidator<OrderStatusDto> _orderStatusValidator;

        public OrderService(
            IOrderRepository orderRepository,
            IMapper mapper,
            IValidator<OrderDto> orderValidator,
            IValidator<OrderItemDto> orderItemValidator,
            IValidator<OrderStatusDto> orderStatusValidator)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _orderValidator = orderValidator;
            _orderItemValidator = orderItemValidator;
            _orderStatusValidator = orderStatusValidator;
        }

        public async Task<OrderDto> CreateOrder(OrderDto orderDto)
        {
            _orderValidator.Validate(orderDto);

            // Fetch and set the OrderStatus from the database
            var orderStatus = await _orderRepository.GetOrderStatusById(orderDto.OrderStatusId);
            if (orderStatus == null)
            {
                throw new ValidationException($"OrderStatus with ID {orderDto.OrderStatusId} not found.");
            }

            // Map the OrderDto to Order, including OrderItems
            var order = _mapper.Map<Order>(orderDto);
            order.OrderStatus = orderStatus;

            // Assign dates and validate each OrderItem within the service
            foreach (var item in order.OrderItems)
            {
                item.CreatedDate = DateTime.UtcNow;
                item.LastUpdatedDate = DateTime.UtcNow;
                _orderItemValidator.Validate(_mapper.Map<OrderItemDto>(item));
            }

            // Save the complete order, including items
            order = await _orderRepository.CreateOrder(order);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
                throw new ValidationException("Order not found.");

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerId(customerId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task UpdateOrder(OrderDto orderDto)
        {
            _orderValidator.Validate(orderDto);
            var order = await _orderRepository.GetOrderById(orderDto.OrderId);
            if (order == null)
                throw new ValidationException("Order not found to update.");

            _mapper.Map(orderDto, order);
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task CancelOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
                throw new ValidationException("Order not found to cancel.");

            await _orderRepository.CancelOrderAsync(orderId);
        }
    }
}