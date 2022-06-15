﻿using System.Data.SqlClient;
using System.Security.Claims;
using Dapper;
using MaxCo.Models;
using MaxCo.Models.ViewModels;

namespace MaxCo.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static string? _connectionString;
        private IHttpContextAccessor _httpContextAccessor;
        private static int activeOrderId;
        public OrderRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;

            if (activeOrderId == 0)
                FindActiveOrder();
            if (activeOrderId == 0)
                CreatOrder();
        }

        public async Task AddOrderProduct(MaxCoViewModels orderProduct)
        {
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
            var sql = @$"SELECT orderProduct.OrderId, ProductName, ProductPrice, Quantity
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

        public Task<MaxCoViewModels> UpdateOrder(MaxCoViewModels orderProduct)
        {
            throw new NotImplementedException();
        }

        private void FindActiveOrder()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
