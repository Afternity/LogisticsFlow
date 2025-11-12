using LogisticsFlow.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsFlow.Persistence.Data.Configurations
{
    public class OrderConfiguration
        : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(order => order.Id);

            builder.HasOne(order => order.Cargo)
                .WithMany(cargo => cargo.Orders)
                .HasForeignKey(order => order.CargoId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(order => order.Employee)
                .WithMany(employee => employee.Orders)
                .HasForeignKey(order => order.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(order => order.Client)
                .WithMany(client => client.Orders)
                .HasForeignKey(order => order.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(order => order.Driver)
                .WithMany(driver => driver.Orders)
                .HasForeignKey(order => order.DriverId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(order => order.Route)
                .WithMany(route => route.Orders)
                .HasForeignKey(order => order.RouteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
