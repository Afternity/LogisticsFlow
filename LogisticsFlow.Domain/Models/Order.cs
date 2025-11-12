using LogisticsFlow.Domain.BaseModels;

namespace LogisticsFlow.Domain.Models
{
    public class Order : BaseModel
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime DesiredDeliveryDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;

        public Guid CargoId { get; set; }
        public virtual Cargo Cargo { get; set; } = null!;  
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;  
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; } = null!;  
        public Guid DriverId { get; set; }
        public virtual Driver Driver { get; set; } = null!;  
        public Guid RouteId { get; set; }
        public virtual Route Route { get; set; } = null!;  
    }
}
