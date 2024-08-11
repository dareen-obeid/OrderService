using Microsoft.EntityFrameworkCore;
using Domain.Models; // Ensure this points to where your domain models are defined

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring Order -> OrderItems relationship
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order) // Ensure the navigation property is explicitly defined
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Specify the delete behavior if needed

            // Configuring Order -> OrderStatus relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.OrderStatus)
                .WithMany(os => os.Orders) // Assuming OrderStatus has a collection of Orders
                .HasForeignKey(o => o.OrderStatusId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion to avoid cascade issues

            // Global Query Filter for IsActive
            modelBuilder.Entity<Order>()
                .HasQueryFilter(o => o.IsActive);

            // Configuring decimal properties
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
    .Property(o => o.TotalPrice)
    .IsRequired()
    .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

        }
    }
}


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Configuring OrderStatus
        //    modelBuilder.Entity<OrderStatus>()
        //        .HasKey(os => os.OrderStatusId);

        //    modelBuilder.Entity<OrderStatus>()
        //        .Property(os => os.OrderStatusName)
        //        .IsRequired()
        //        .HasMaxLength(50);

        //    // Configuring Order
        //    modelBuilder.Entity<Order>()
        //        .HasKey(o => o.OrderId);

        //    modelBuilder.Entity<Order>()
        //        .HasMany(o => o.OrderItems)
        //        .WithOne(oi => oi.Order)
        //        .HasForeignKey(oi => oi.OrderId)
        //        .OnDelete(DeleteBehavior.Cascade); // Ensuring single cascade path

        //    modelBuilder.Entity<Order>()
        //        .HasOne(o => o.OrderStatus)
        //        .WithMany()
        //        .HasForeignKey(o => o.OrderStatusId)
        //        .IsRequired()
        //        .OnDelete(DeleteBehavior.Restrict); // Avoiding multiple cascade paths

        //    modelBuilder.Entity<Order>()
        //        .Property(o => o.TotalPrice)
        //        .HasColumnType("decimal(18, 2)");

        //    modelBuilder.Entity<Order>()
        //        .HasQueryFilter(o => o.IsActive); // Applying global query filter

        //    // Configuring OrderItem
        //    modelBuilder.Entity<OrderItem>()
        //        .HasKey(oi => oi.OrderItemId);

        //    modelBuilder.Entity<OrderItem>()
        //        .Property(oi => oi.Price)
        //        .HasColumnType("decimal(18, 2)");

        //    modelBuilder.Entity<OrderItem>()
        //        .HasQueryFilter(oi => oi.IsActive); // Applying global query filter aligning with Order
        //}
