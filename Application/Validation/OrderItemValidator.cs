using System;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Application.Validation
{
    public class OrderItemValidator : IValidator<OrderItemDto>
    {
        public void Validate(OrderItemDto item)
        {
            if (item == null)
                throw new ValidationException("Order item cannot be null.");

            if (item.Quantity <= 0)
                throw new ValidationException("Quantity must be greater than zero.");

            if (item.Price <= 0m)
                throw new ValidationException("Price must be greater than zero.");

            if (item.ProductId <= 0)
                throw new ValidationException("Product ID must be valid.");
        }
    }
}

