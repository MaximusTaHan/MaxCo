using MaxCo.Models.ViewModels;

namespace MaxCo.Repositories
{
    public interface IProductRepository
    {
        Task<MaxCoViewModels> GetAll();
        Task<MaxCoViewModels> GetDetailed(int productId);
    }
}