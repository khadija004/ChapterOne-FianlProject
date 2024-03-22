using ChapterOneApp.Service.Interfaces;
using ChapterOneApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly ILayoutService _layoutService;
        private readonly ICartService _cartService;
        private readonly IWishlistService _wishlistService;

        public HeaderViewComponent(ILayoutService layoutService,
                           ICartService cartService,
                           IWishlistService wishlistService)
        {
            _layoutService = layoutService;
            _cartService = cartService;
            _wishlistService = wishlistService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            LayoutVM model = new()
            {
                Settings = _layoutService.GetSettingsData(),
                BasketCount = _cartService.GetDatasFromCookie().Count,
                WishlistCount = _wishlistService.GetDatasFromCookie().Count,
            };

            return await Task.FromResult(View(model));
        }
    }
}
