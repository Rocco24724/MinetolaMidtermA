using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinetolaMidtermA.Data;

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
    }
}