using Microsoft.AspNetCore.Mvc;
using MaxCo.Models.ViewModels;
using MaxCo.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MaxCo.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Order()
        {
            var order = await _orderRepository.GetOrder();

            return View(order);
        }

        public async Task<IActionResult> AddProduct(MaxCoViewModels addProduct)
        {
            await _orderRepository.AddOrderProduct(addProduct);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateOrder(MaxCoViewModels adjustOrderProduct)
        {
           var adjustedOrder = await _orderRepository.UpdateOrder(adjustOrderProduct);

            return View();
        }

        public async Task<IActionResult> Delete(MaxCoViewModels product)
        {
            int id = product.OrderProducts[0].OrderId;
            await _orderRepository.DeleteOrder(id);
            return RedirectToAction("Order");
        }
    }
}
