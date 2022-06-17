using MaxCo.Models;
using MaxCo.Models.ViewModels;

namespace MaxCo.Repositories
{
    public interface IOrderRepository
    {
        Task<MaxCoViewModels> GetOrder();
        Task AddOrderProduct(MaxCoViewModels orderProduct);
        Task UpdateOrder(OrderProductModel orderProduct);
        Task DeleteOrder(int orderId);
        Task DeleteItem(int? itemId);
    }
}