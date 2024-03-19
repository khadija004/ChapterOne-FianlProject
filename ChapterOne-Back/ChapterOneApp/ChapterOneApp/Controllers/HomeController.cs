using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service;
using ChapterOneApp.Service.Interfaces;
using ChapterOneApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChapterOneApp.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;
        readonly ISliderService _sliderService;
        readonly IOurService _ourService;
        readonly ITeamService _teamService;
        readonly IAutobiographyOneService _autobiographyOneService;
        readonly IAutobiographyTwoService _autobiographyTwoService;
        readonly IBrandService _brandService;
        readonly IProductService _produtcService;
        readonly IGenreService _genreService;



        public HomeController(AppDbContext context, 
                                ISliderService sliderService,        
                                IOurService ourService,
                                ITeamService teamService,
                                IAutobiographyOneService autobiographyOneService,
                                IAutobiographyTwoService autobiographyTwoService,
                                IBrandService brandService,
                                IProductService productService,
                                IGenreService genreService)
        {
            _context = context;
            _sliderService = sliderService;
            _teamService = teamService;
            _ourService = ourService;
            _autobiographyOneService = autobiographyOneService;
            _autobiographyTwoService = autobiographyTwoService;
            _brandService = brandService;
            _produtcService = productService;
            _genreService = genreService;


        }


        public async Task<IActionResult> Index()
        {

            List<Slider> sliders = await _sliderService.GetAll();
            List<Our> ours = await _ourService.GetAllAsync();
            List<Team> teams = await _teamService.GetAllAsync();
            List<AutobiographyOne> autobiographyOnes = await _autobiographyOneService.GetAllAsync();
            List<AutobiographyTwo> autobiographyTwos = await _autobiographyTwoService.GetAllAsync();
            List<Brand> brands = await _brandService.GetAllAsync();
            List<Product> products = await _produtcService.GetAll();
            List<Genre> genres = await _genreService.GetAllAsync();
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);




            HomeVM model = new()
            {
                Sliders = sliders,
                Ours = ours,
                Teams = teams,
                AutobiographyOnes = autobiographyOnes,
                AutobiographyTwos = autobiographyTwos,
                HeaderBackgrounds = headerBackground,
                Brands = brands,
                Products = products,
                Genres = genres
            };

            return View(model);
        }

     
    }
}