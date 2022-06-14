using System.Data.SqlClient;
using Dapper;
using MaxCo.Models;
using MaxCo.Models.ViewModels;

namespace MaxCo.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static string? _connectionString;
        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Task<MaxCoViewModels> AddOrderProduct(MaxCoViewModels OrderProduct)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<MaxCoViewModels> GetOrder()
        {
            var sql = @"SELECT orderProduct.OrderId, ProductName, ProductPrice
                        FROM orderProduct
                        INNER JOIN orders ON orderProduct.OrderId = orders.OrderId
                        INNER JOIN products ON orderProduct.ProductKey = products.ProductId";

            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                products.OrderProducts = (List<OrderProductModel>)await connection.QueryAsync<OrderProductModel>(sql);
            }

            return products;
        }

        public Task<MaxCoViewModels> UpdateOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
