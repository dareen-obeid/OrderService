using System;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Application.Validation
{
    public class OrderValidator : IValidator<OrderDto>
    {
        public void Validate(OrderDto order)
        {
            if (order == null)
                throw new ValidationException("Order cannot be null.");

            if (order.TotalPrice <= 0)
                throw new ValidationException("Total price must be greater than zero.");

            if (order.CreatedDate == default)
                throw new ValidationException("Creation date is required.");

            if (order.OrderItems == null || !order.OrderItems.Any())
                throw new ValidationException("Order must contain at least one item.");

            if (order.OrderStatusId <= 0)
                throw new ValidationException("Order status ID must be valid.");
        }
    }
}

