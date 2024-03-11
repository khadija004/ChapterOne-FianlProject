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
        private readonly IAutobiographyOneService _autobiographyOneService;
        private readonly IAutobiographyTwoService _autobiographyTwoService;


        public HomeController(AppDbContext context, 
                                ISliderService sliderService,        
                                IOurService ourService,
                                ITeamService teamService,
                                IAutobiographyOneService autobiographyOneService,
                                IAutobiographyTwoService autobiographyTwoService)
        {
            _context = context;
            _sliderService = sliderService;
            _teamService = teamService;
            _ourService = ourService;
            _autobiographyOneService = autobiographyOneService;
            _autobiographyTwoService = autobiographyTwoService;

        }


        public async Task<IActionResult> Index()
        {

            List<Slider> sliders = await _sliderService.GetAll();
            List<Our> ours = await _ourService.GetAllAsync();
            List<Team> teams = await _teamService.GetAllAsync();
            List<AutobiographyOne> autobiographyOnes = await _autobiographyOneService.GetAllAsync();
            List<AutobiographyTwo> autobiographyTwos = await _autobiographyTwoService.GetAllAsync();


            HomeVM model = new()
            {
                Sliders = sliders,
                Ours = ours,
                Teams = teams,
                AutobiographyOnes = autobiographyOnes,
                AutobiographyTwos = autobiographyTwos,
            };

            return View(model);
        }

     
    }
}