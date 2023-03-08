using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;

namespace Web.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketViewModelService _basketViewModelService;

        public BasketController(IBasketViewModelService basketViewModelService)
        {
            _basketViewModelService = basketViewModelService;
        }
        public async Task<IActionResult> AddToBasket(int productId)
        {
            var vm = await _basketViewModelService.AddItemToBasketAsync(productId, 1);
            return Json(vm);
        }
    }
}
