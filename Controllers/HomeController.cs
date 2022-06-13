using System.Diagnostics;
using MaxCo.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using MaxCo.Models.ViewModels;

namespace MaxCo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var sql = "select * from products";

            string connString = _configuration.GetConnectionString("DefaultConnection");
            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                products.Products = (List<ProductModel>)connection.Query<ProductModel>(sql);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult OrdersTest()
        {
            var sql = "select * from orders";

            string connString = _configuration.GetConnectionString("DefaultConnection");
            var products = new MaxCoViewModels();

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                products.Orders = (List<OrdersModel>)connection.Query<OrdersModel>(sql);
            }

            return View();
        }

        public IActionResult OrderProductTest()
        {
            var sql = @"SELECT orderProduct.OrderId, ProductName, ProductDescription
                        FROM orderProduct
                        INNER JOIN orders ON orderProduct.OrderId = orders.OrderId
                        INNER JOIN products ON orderProduct.ProductKey = products.ProductId";

            string connString = _configuration.GetConnectionString("DefaultConnection");
            var orderProducts = new MaxCoViewModels();

            using (var connection = new SqlConnection(connString))
            {
                orderProducts.OrderProducts = (List<OrderProductModel>)connection.Query<OrderProductModel>(sql);
            }

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}