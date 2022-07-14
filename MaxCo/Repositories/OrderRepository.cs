using System.Data.SqlClient;
using System.Security.Claims;
using Coravel.Queuing.Interfaces;
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
        private readonly IEmailSender _emailSender;
        private static int activeOrderId;
        public OrderRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;

            if (activeOrderId == 0)
                FindActiveOrder();
            if (activeOrderId == 0)
                CreatOrder();
        }

        public async Task AddOrderProduct(MaxCoViewModels orderProduct)
        {
            FindActiveOrder();

            string? sql = $@"IF EXISTS (SELECT * FROM orderProduct WHERE ProductKey = @ProductKey AND orderProduct.OrderId = @OrderId)
                            BEGIN
	                            UPDATE orderProduct
	                            SET Quantity = Quantity + 1
	                            WHERE ProductKey = @ProductKey
                            END
                            ELSE
                            BEGIN
	                            INSERT INTO orderProduct VALUES (@OrderId, @ProductKey, 1)
                            END";
            var parameters = new { ProductKey = orderProduct.Product.ProductId, OrderId = activeOrderId };

            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            await connection.ExecuteAsync(sql, parameters);
        }

        public Task DeleteOrder(int orderId)
        {
            FindActiveOrder();
            var sql = $@"DELETE FROM orders
                        WHERE OrderId = @OrderId";

            var parameters = new { OrderId = orderId };
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql, parameters);
            }
            activeOrderId = 0;
            return Task.CompletedTask;
        }

        public async Task<MaxCoViewModels> GetOrder()
        {
            FindActiveOrder();
            var sql = @$"SELECT orderProduct.OrderId, ProductName, ProductPrice, ProductId, ProductImage, Quantity
	                        FROM orderProduct
	                        INNER JOIN products ON orderProduct.ProductKey = products.ProductId
	                        WHERE orderProduct.OrderId = @OrderId";
            var parameters = new { OrderId = activeOrderId };
            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                products.OrderProducts = (List<OrderProductModel>)await connection.QueryAsync<OrderProductModel>(sql, parameters);
            }

            return products;
        }

        public Task UpdateOrder(int quantity, int productId)
        {
            FindActiveOrder();
            string sql = @$"UPDATE orderProduct
                            SET Quantity = @Quantity
                            WHERE ProductKey = @ProductKey
                            AND OrderId = @OrderId";
            var parameters = new { Quantity = quantity, ProductKey = productId, OrderId = activeOrderId };

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql, parameters);
            }
            return Task.CompletedTask;
        }

        public Task DeleteItem(int? itemId)
        {
            FindActiveOrder();
            string sql = $@"DELETE FROM orderProduct
                            WHERE ProductKey = @ProductKey";
            var parameters = new { ProductKey = itemId };
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql, parameters);
            }
            return Task.CompletedTask;
        }

        public async Task ConfirmOrder()
        {
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var finalOrder = await GetOrder();
            decimal? total = 0;

            foreach (var order in finalOrder.OrderProducts)
            {
                total += order.Quantity * order.ProductPrice;
            }

            FinalizedOrder final = new()
            {
                TotalPrice = total,
                CustomerEmail = userEmail,
                OrderProducts = finalOrder.OrderProducts
            };

            Task.Run(() => _emailSender.ConfirmationSender(final));

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sql = $@"UPDATE orders
                        SET OrderStatus = 1
                        WHERE OrderStatus = 2
                        AND UserId = '{userId}'";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute(sql);
        }

        private void FindActiveOrder()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sql = @$"SELECT OrderId
                        FROM orders
                        WHERE OrderStatus = 2
                        AND UserId = '{userId}'";

            using var connections = new SqlConnection(_connectionString);
            connections.Open();
            activeOrderId = connections.ExecuteScalar<int>(sql);
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
