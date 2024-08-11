using Application.DTOs;
using Application.Mappings;
using Application.Services;
using Application.Services.Interfaces;
using Application.Validation;
using Domain.RepositoriyInterfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using OrderService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Add the DbContext service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("Infrastructure")
    ));


// Register AutoMapper and specify the assembly containing the profiles
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


builder.Services.AddSingleton<IValidator<OrderDto>, OrderValidator>();
builder.Services.AddSingleton<IValidator<OrderItemDto>, OrderItemValidator>();
builder.Services.AddSingleton<IValidator<OrderStatusDto>, OrderStatusValidator>();

// Register repositories and services
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, Application.Services.OrderService>();




var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

