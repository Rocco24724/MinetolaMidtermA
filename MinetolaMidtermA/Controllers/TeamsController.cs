using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinetolaMidtermA.Data;
using MinetolaMidtermA.Models;

namespace MinetolaMidtermA.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _context.Team.ToListAsync();
            return View(teams);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }

            _context.Team.Add(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}