using Microsoft.EntityFrameworkCore;
using LogisticsFlow.Domain.Models;
using LogisticsFlow.Persistence.Data.Configurations;

namespace LogisticsFlow.Persistence.Data.DbContexts
{
    public class LogisticsFlowDbContext
        : DbContext
    {
        public LogisticsFlowDbContext(
            DbContextOptions<LogisticsFlowDbContext> options)
            : base(options)
        {
        }

        public LogisticsFlowDbContext()
        {
        }

        public virtual DbSet<Cargo> Cargos { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=AFTERNITY;Initial Catalog=LogisticsFlowDB;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CargoConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new DriverConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new RouteConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
        }
    }
}
