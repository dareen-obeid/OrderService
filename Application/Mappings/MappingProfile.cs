using System;
using Application.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
	public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderStatus, OrderStatusDto>().ReverseMap();


  


    }
	}
}

