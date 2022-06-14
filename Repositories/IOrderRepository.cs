using MaxCo.Models.ViewModels;

namespace MaxCo.Repositories
{
    public interface IOrderRepository
    {
        Task<MaxCoViewModels> GetOrder();
        Task<MaxCoViewModels> AddOrderProduct(MaxCoViewModels OrderProduct);
        Task<MaxCoViewModels> UpdateOrder(int orderId);
        Task DeleteOrder(int orderId);

    }
}