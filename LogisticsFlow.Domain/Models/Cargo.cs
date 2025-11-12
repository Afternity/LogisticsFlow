using LogisticsFlow.Domain.BaseModels;

namespace LogisticsFlow.Domain.Models
{
    public class Cargo : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; } = decimal.Zero;
        public decimal Volume { get; set; } = decimal.Zero;

        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
