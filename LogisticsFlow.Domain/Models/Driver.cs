using LogisticsFlow.Domain.BaseModels;

namespace LogisticsFlow.Domain.Models
{
    public class Driver : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;

        public Guid VehicleID { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
