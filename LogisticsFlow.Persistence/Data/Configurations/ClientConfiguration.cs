using LogisticsFlow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsFlow.Persistence.Data.Configurations
{
    public class ClientConfiguration 
        : IEntityTypeConfiguration<Client>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(client => client.Id);

            builder.HasMany(client => client.Orders)
                   .WithOne(order => order.Client)
                   .HasForeignKey(order => order.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
