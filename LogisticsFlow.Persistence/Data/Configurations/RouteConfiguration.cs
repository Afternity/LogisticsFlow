using LogisticsFlow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsFlow.Persistence.Data.Configurations
{
    public class RouteConfiguration
        : IEntityTypeConfiguration<Route>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Route> builder)
        {
            builder.HasKey(route => route.Id);

            builder.HasMany(route => route.Orders)
                   .WithOne(order => order.Route)
                   .HasForeignKey(order => order.RouteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
