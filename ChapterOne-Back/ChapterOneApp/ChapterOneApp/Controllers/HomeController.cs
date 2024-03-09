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


        public HomeController(AppDbContext context, 
                                ISliderService sliderService,        
                                IOurService ourService,
                                ITeamService teamService)
        {
            _context = context;
            _sliderService = sliderService;
            _teamService = teamService;
            _ourService = ourService;

        }


        public async Task<IActionResult> Index()
        {

            List<Slider> sliders = await _sliderService.GetAll();
            List<Our> ours = await _ourService.GetAllAsync();
            List<Team> teams = await _teamService.GetAllAsync();


            HomeVM model = new()
            {
                Sliders = sliders,
                Ours = ours,
                Teams = teams,
            };

            return View(model);
        }

     
    }
}