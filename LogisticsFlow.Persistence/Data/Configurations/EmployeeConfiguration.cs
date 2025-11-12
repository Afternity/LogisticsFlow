using LogisticsFlow.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace LogisticsFlow.Persistence.Data.Configurations
{
    public class EmployeeConfiguration 
        : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(employee => employee.Id);

            builder.HasMany(employee => employee.Orders)
                   .WithOne(order => order.Employee)
                   .HasForeignKey(order => order.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
