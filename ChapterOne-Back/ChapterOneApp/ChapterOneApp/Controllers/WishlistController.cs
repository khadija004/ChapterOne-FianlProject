using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using ChapterOneApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWishlistService _wishlistService;
        public WishlistController(IProductService productService, IWishlistService wishlistService)
        {
            _productService = productService;
            _wishlistService = wishlistService;
        }
        public async Task<IActionResult> Index()
        {
            List<WishlistVM> wishlists = _wishlistService.GetDatasFromCookie();
            List<WishlistDetailVM> wishlistDetailVMs = new();
            foreach (var item in wishlists)
            {
                Product dbProduct = await _productService.GetById((int)item.ProductId);

                wishlistDetailVMs.Add(new WishlistDetailVM()
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Price = dbProduct.Price,
                    Image = dbProduct.Image,
                });
            }
            return View(wishlistDetailVMs);
        }


        [HttpPost]
        public IActionResult DeleteDataFromWishlist(int? id)
        {
            if (id is null) return BadRequest();

            _wishlistService.DeleteData((int)id);
            List<WishlistVM> wishlists = _wishlistService.GetDatasFromCookie();

            return Ok(wishlists.Count);

        }
    }
}
