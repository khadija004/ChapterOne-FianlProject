using ChapterOneApp.Areas.Admin.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Helpers;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITeamService _teamService;
        private readonly IPositionService _positionService;
        private readonly IWebHostEnvironment _env;
        public TeamController(AppDbContext context,
                              ITeamService teamService,
                              IWebHostEnvironment env,
                              IPositionService positionService)
        {
            _context = context;
            _teamService = teamService;
            _env = env;
            _positionService = positionService;
        }


        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _teamService.GetAllAsync();
            return View(teams);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Team dbTeam = await _teamService.GetFullDataByIdAsync(id);
            if (dbTeam is null) return NotFound();
            return View(dbTeam);
        }


        private async Task<SelectList> GetPositionAsync()
        {
            List<Position> positions = await _positionService.GetAllAsync();
            return new SelectList(positions, "Id", "Name");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            @ViewBag.positions = await GetPositionAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamCreateVM createTeam)
        {
            try
            {
                @ViewBag.positions = await GetPositionAsync();

                if (!ModelState.IsValid)
                {
                    return View(createTeam);
                }

                if (!createTeam.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View(createTeam);
                }

                if (!createTeam.Photo.CheckFileSize(600))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 600kb");
                    return View(createTeam);
                }

                string fileName = Guid.NewGuid().ToString() + "_" + createTeam.Photo.FileName;
                string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName);
                await FileHelper.SaveFileAsync(newPath, createTeam.Photo);

                Team newTeam = new()
                {
                    Image = fileName,
                    Name = createTeam.Name,
                    PositionId = createTeam.PositionId,
                    Description = createTeam.Description,
                };

                await _context.Teams.AddAsync(newTeam);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                Team dbTeam = await _teamService.GetFullDataByIdAsync(id);
                if (dbTeam == null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbTeam.Image);
                FileHelper.DeleteFile(path);

                _context.Teams.Remove(dbTeam);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                throw;
            }

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                @ViewBag.positions = await GetPositionAsync();

                if (id == null) return BadRequest();
                Team dbTeam = await _teamService.GetFullDataByIdAsync(id);
                if (dbTeam == null) return NotFound();

                TeamUpdateVM model = new()
                {
                    Image = dbTeam.Image,
                    Name = dbTeam.Name,
                    Description = dbTeam.Description,
                    PositionId = dbTeam.PositionId
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, TeamUpdateVM updatedTeam)
        {
            try
            {
                @ViewBag.positions = await GetPositionAsync();

                if (id == null) return BadRequest();
                Team dbTeam = await _teamService.GetFullDataByIdAsync(id);
                if (dbTeam == null) return NotFound();

                TeamUpdateVM model = new()
                {
                    Image = dbTeam.Image,
                    Name = dbTeam.Name,
                    Description = dbTeam.Description,
                    PositionId = dbTeam.PositionId
                };

                if (updatedTeam.Photo != null)
                {
                    if (!updatedTeam.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(model);
                    }
                    if (!updatedTeam.Photo.CheckFileSize(600))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 600kb");
                        return View(model);
                    }

                    string oldPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbTeam.Image);
                    FileHelper.DeleteFile(oldPath);

                    string fileName = Guid.NewGuid().ToString() + " " + updatedTeam.Photo.FileName;
                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName);
                    await FileHelper.SaveFileAsync(newPath, updatedTeam.Photo);
                    dbTeam.Image = fileName;
                }
                else
                {
                    Team newTeam = new()
                    {
                        Image = dbTeam.Image
                    };
                }

                dbTeam.Name = updatedTeam.Name;
                dbTeam.PositionId = updatedTeam.PositionId;
                dbTeam.Description = updatedTeam.Description;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }
    }
}
