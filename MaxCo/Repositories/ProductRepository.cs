using System.Data.SqlClient;
using Dapper;
using MaxCo.Models;
using MaxCo.Models.ViewModels;

namespace MaxCo.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static string? _connectionString;
        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<MaxCoViewModels> GetAll()
        {
            var sql = "select * from products";

            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                products.Products = (List<ProductModel>) await connection.QueryAsync<ProductModel>(sql);
            }

            return products;
        }

        public async Task<MaxCoViewModels> GetDetailed(int productId)
        {
            string sql = $"select * from products WHERE productId = @ProductId";
            var param = new { ProductId = productId };

            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                products.Product = await connection.QueryFirstAsync<ProductModel>(sql, param);
            }

            return products;
        }
    }
}
