using System.Data.SqlClient;
using System.Security.Claims;
using Dapper;
using MaxCo.Models;
using MaxCo.Models.ViewModels;
using MaxCoEmailService;

namespace MaxCo.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static string? _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProcessOrder _processOrder;
        private static int activeOrderId;
        public OrderRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IProcessOrder processOrder)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
            _processOrder = processOrder;

            if (activeOrderId == 0)
                FindActiveOrder();
            if (activeOrderId == 0)
                CreatOrder();
        }

        public async Task AddOrderProduct(MaxCoViewModels orderProduct)
        {
            FindActiveOrder();
            string? sql = $@"IF EXISTS (SELECT * FROM orderProduct WHERE ProductKey = {orderProduct.Product.ProductId} AND orderProduct.OrderId = {activeOrderId})
                            BEGIN
	                            UPDATE orderProduct
	                            SET Quantity = Quantity + 1
	                            WHERE ProductKey = {orderProduct.Product.ProductId}
                            END
                            ELSE
                            BEGIN
	                            INSERT INTO orderProduct VALUES ({activeOrderId}, {orderProduct.Product.ProductId}, 1)
                            END";

            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            await connection.ExecuteAsync(sql);
        }

        public Task DeleteOrder(int orderId)
        {
            FindActiveOrder();
            var sql = $@"DELETE FROM orders
                        WHERE OrderId = {orderId}";
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql);
            }
            activeOrderId = 0;
            return Task.CompletedTask;
        }

        public async Task<MaxCoViewModels> GetOrder()
        {
            FindActiveOrder();
            var sql = @$"SELECT orderProduct.OrderId, ProductName, ProductPrice, ProductId, Quantity
	                        FROM orderProduct
	                        INNER JOIN products ON orderProduct.ProductKey = products.ProductId
	                        WHERE orderProduct.OrderId = {activeOrderId}";

            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                products.OrderProducts = (List<OrderProductModel>)await connection.QueryAsync<OrderProductModel>(sql);
            }

            return products;
        }

        public Task UpdateOrder(OrderProductModel orderProduct)
        {
            FindActiveOrder();
            string sql = @$"UPDATE orderProduct
                            SET Quantity = {orderProduct.Quantity}
                            WHERE ProductKey = {orderProduct.ProductId}
                            AND OrderId = {activeOrderId}";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql);
            }
            return Task.CompletedTask;
        }

        public Task DeleteItem(int? itemId)
        {
            FindActiveOrder();
            string sql = $@"DELETE FROM orderProduct
                            WHERE ProductKey = {itemId}";

            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql);
            }
            return Task.CompletedTask;
        }

        private void FindActiveOrder()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var dummyObject = new OrderProductModel
            {

            };

            _processOrder.ConfirmationSender(dummyObject);
            var sql = @$"SELECT OrderId
                        FROM orders
                        WHERE OrderStatus = 2
                        AND UserId = '{userId}'";

            using (var connections = new SqlConnection(_connectionString))
            {
                connections.Open();
                activeOrderId = connections.ExecuteScalar<int>(sql);
            }
        }

        private void CreatOrder()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sql = @$"INSERT INTO orders VALUES (2, 2022-06-15, '{userId}', 2)";

            using(var connections = new SqlConnection(_connectionString))
            {
                connections.Open();
                connections.Execute(sql);
            }

            FindActiveOrder();
        }
    }
}
