using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TiendaWep.Data;
using TiendaWep.Models;

namespace TiendaWep.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cervezas = _context.Cervezas.Include(c =>c.Estilo);
            return View(cervezas.ToListAsync());
        }

        public async Task<IActionResult> Deatalles(int? id)
        {
            var cervezas = await _context.Cervezas.Include(c => c.Estilo).FirstOrDefaultAsync(c => c.Id ==id);
            return View(cervezas);
        }

        public IActionResult Privacy()
        {
      
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
