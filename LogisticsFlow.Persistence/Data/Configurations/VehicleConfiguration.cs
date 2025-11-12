using LogisticsFlow.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsFlow.Persistence.Data.Configurations
{
    public class VehicleConfiguration
        : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(vehicle => vehicle.Id);

            builder.HasMany(vehicle => vehicle.Drivers)
                   .WithOne(driver => driver.Vehicle)
                   .HasForeignKey(driver => driver.VehicleID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
