﻿using Microsoft.AspNetCore.Mvc;
using MaxCo.Models.ViewModels;
using MaxCo.Repositories;

namespace MaxCo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var products = new MaxCoViewModels();
            if(string.IsNullOrEmpty(searchString))
            {
                products = await _productRepository.GetAll();
            }
            else
            {
                products = await _productRepository.GetFiltered(searchString);
            }

            return View(products);
        }

        public async Task<IActionResult> Category(string categorySearch)
        {
            var products = await _productRepository.GetCategory(categorySearch);

            return View(products);
        }
        public async Task<IActionResult> Details(MaxCoViewModels id)
        {
            var viewId = id.Product.ProductId;
            var product = await _productRepository.GetDetailed(viewId);

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}