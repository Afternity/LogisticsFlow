using Microsoft.Extensions.DependencyInjection;
using LogisticsFlow.Persistence.Data.DependencyInjections;

namespace LogisticsFlow.ContractWPF.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContractWPF(
          this IServiceCollection services)
        {
            services.AddPersistence("Data Source=AFTERNITY;Initial Catalog=LogisticsFlowDB;Integrated Security=True;Trust Server Certificate=True");

            return services;
        }
    }
}
