using Microsoft.AspNetCore.Mvc;
using MaxCo.Models.ViewModels;
using MaxCo.Repositories;
using Microsoft.AspNetCore.Authorization;
using MaxCo.Models;
using MaxCoEmailService;

namespace MaxCo.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        //private readonly Worker _worker;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            //_worker = worker;
        }

        public async Task<IActionResult> Order()
        {
            var order = await _orderRepository.GetOrder();

            foreach (var orderItem in order.OrderProducts)
            {
                decimal? price = orderItem.ProductPrice;
                int? quantity = orderItem.Quantity;

                orderItem.Total = quantity * price;
            }

            return View(order);
        }

        public async Task<IActionResult> AddProduct(MaxCoViewModels addProduct)
        {
            await _orderRepository.AddOrderProduct(addProduct);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateOrder([Bind(Prefix = "order")]OrderProductModel adjustOrderProduct)
        {
           await _orderRepository.UpdateOrder(adjustOrderProduct);

            return RedirectToAction("Order");
        }

        public async Task<IActionResult> Delete(MaxCoViewModels fullOrder)
        {
            int id = fullOrder.OrderProducts[0].OrderId;
            await _orderRepository.DeleteOrder(id);
            return RedirectToAction("Order");
        }

        public async Task<IActionResult> DeleteItem([Bind(Prefix = "order")] OrderProductModel adjustOrderProduct)
        {
            await _orderRepository.DeleteItem(adjustOrderProduct.ProductId);
            return RedirectToAction("Order");
        }
    }
}
