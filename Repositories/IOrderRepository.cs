using MaxCo.Models.ViewModels;

namespace MaxCo.Repositories
{
    public interface IOrderRepository
    {
        Task<MaxCoViewModels> GetOrder();
        Task AddOrderProduct(MaxCoViewModels orderProduct);
        Task<MaxCoViewModels> UpdateOrder(MaxCoViewModels orderProduct);
        Task DeleteOrder(int orderId);
    }
}