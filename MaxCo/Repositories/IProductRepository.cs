using MaxCo.Models.ViewModels;

namespace MaxCo.Repositories
{
    public interface IProductRepository
    {
        Task<MaxCoViewModels> GetAll();
        Task<MaxCoViewModels> GetDetailed(int productId);
        Task<MaxCoViewModels> GetFiltered(string id);
        Task<MaxCoViewModels> GetCategory(string categorySearch);
    }
}