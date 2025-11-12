namespace LogisticsFlow.Domain.Interfaces
{
    public interface IBaseCRUD
    {
        Task CreateAsync();
        Task UpdateAsync();
        Task DeleteAsync();
        Task GetAllAsync();
    }
}
