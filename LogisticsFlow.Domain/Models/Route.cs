using LogisticsFlow.Domain.BaseModels;

namespace LogisticsFlow.Domain.Models
{
    public class Route : BaseModel
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public double Distance { get; set; } = 0.0;
        public TimeSpan EstimatedTime { get; set; } = TimeSpan.Zero;

        public string OriginDestination => $"{Origin} → {Destination}";
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
