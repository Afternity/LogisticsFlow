using LogisticsFlow.Domain.BaseModels;

namespace LogisticsFlow.Domain.Models
{
    public class Vehicle : BaseModel
    {
        public string Mark { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public decimal LoadCapacity { get; set; } = decimal.Zero;

        public virtual ICollection<Driver> Drivers { get; set; } = [];
    }
}
