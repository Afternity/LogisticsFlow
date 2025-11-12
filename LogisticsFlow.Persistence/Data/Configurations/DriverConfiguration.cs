using LogisticsFlow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsFlow.Persistence.Data.Configurations
{
    public class DriverConfiguration 
        : IEntityTypeConfiguration<Driver>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Driver> builder)
        {
            builder.HasKey(driver => driver.Id);

            builder.HasOne(driver => driver.Vehicle)
                   .WithMany(vehicle => vehicle.Drivers)
                   .HasForeignKey(driver => driver.VehicleID)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(driver => driver.Orders)
                   .WithOne(order => order.Driver)
                   .HasForeignKey(order => order.DriverId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
