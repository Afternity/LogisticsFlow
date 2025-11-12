using Microsoft.EntityFrameworkCore;
using LogisticsFlow.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsFlow.Persistence.Data.Configurations
{
    public class CargoConfiguration 
        : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            builder.HasKey(cargo => cargo.Id);

            builder.HasMany(cargo => cargo.Orders)
                   .WithOne(order => order.Cargo)
                   .HasForeignKey(order => order.CargoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
