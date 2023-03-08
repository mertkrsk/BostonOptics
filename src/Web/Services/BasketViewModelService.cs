using System.Security.Claims;
using Web.Extensions;
using Web.Interfaces;
using Web.Models;

namespace Web.Services
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketViewModelService(IBasketService basketService, IHttpContextAccessor httpContextAccessor)
        {
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpContext HttpContext => _httpContextAccessor.HttpContext!;
        public string? UserId => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string? AnonId => _createdAnonId ?? HttpContext.Request.Cookies[Constants.BASKET_COOKIENAME];
        public string BuyerId => UserId ?? AnonId ?? CreateAnonymousId();

        private string _createdAnonId;

        private string CreateAnonymousId()
        {
            _createdAnonId = Guid.NewGuid().ToString();
            HttpContext.Response.Cookies.Append(Constants.BASKET_COOKIENAME, _createdAnonId, new CookieOptions()
            {
                Expires = DateTime.Now.AddMonths(2),
                IsEssential = true
            });
            return _createdAnonId;
        }

        public async Task<BasketViewModel> AddItemToBasketAsync(int productId, int quantity)
        {
            var basket = await _basketService.AddItemToBasketAsync(BuyerId, productId, quantity);
            return basket.ToBasketViewModel();

        }

        public async Task<BasketViewModel> GetBasketViewModelAsync()
        {
           var basket=await _basketService.GetOrCreateBasketAsync(BuyerId);
            return basket.ToBasketViewModel();
        }
    }
}
