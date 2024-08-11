using System;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Application.Validation
{
    public class OrderStatusValidator : IValidator<OrderStatusDto>
    {
        public void Validate(OrderStatusDto status)
        {
            if (string.IsNullOrWhiteSpace(status.OrderStatusName))
                throw new ValidationException("Order status name is required.");

            if (status.OrderStatusName.Length > 50)
                throw new ValidationException("Order status name must be up to 50 characters long.");
        }
    }
}

