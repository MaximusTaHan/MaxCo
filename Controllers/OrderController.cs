using Microsoft.AspNetCore.Mvc;
using MaxCo.Models.ViewModels;
using MaxCo.Repositories;

namespace MaxCo.Controllers
{
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
    }
}
