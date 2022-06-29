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

        public async Task<MaxCoViewModels> GetCategory(string categorySearch)
        {
            string sql = $@"SELECT products.ProductId, products.ProductName, products.ProductPrice, products.ProductDescription, products.ProductImage, products.ProductBrand
                            FROM products
                            INNER JOIN productCategories
                            ON products.ProductId = productCategories.ProductKey
                            WHERE productCategories.CategoryKey = '{categorySearch}';";

            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                products.Products = (List<ProductModel>)await connection.QueryAsync<ProductModel>(sql);
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

        public async Task<MaxCoViewModels> GetFeatureProducts()
        {
            string sql = @$"select * FROM featureProducts";

            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                products.FeaturedProducts = (List<FeatureProductModel>) await connection.QueryAsync<FeatureProductModel>(sql);
            }
            return products;
        }

        public async Task<MaxCoViewModels> GetFiltered(string id)
        {
            string sql = @$"select * from products WHERE ProductName LIKE '%{id}%'
                            OR ProductDescription LIKE'%{id}%'
                            OR ProductBrand LIKE '%{id}%';";

            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                products.Products = (List<ProductModel>)await connection.QueryAsync<ProductModel>(sql);
            }

            return products;
        }
    }
}
