using ExpressVoitures.Data;
using ExpressVoitures.Models;
using ExpressVoitures.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExpressVoitures.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }

        /*Nos voitures*/
        public async Task<IActionResult> NosVoitures()
        {
            List<Voiture> voitures = await _context.Voitures
            .Include(v => v.Reparations)
            .ToListAsync();

            List<VoitureVM> voitureVMs = voitures.Select(v => new VoitureVM(v, _context)).ToList();

            foreach (var vm in voitureVMs)
            {
                vm.CalculTotalReparation();
                vm.CalculStatutVoiture();

                if (vm.Voiture.PrixDeVente == null)
                {
                    vm.Voiture.PrixDeVente = vm.CalculPrixVente();
                }
            }
            return View(voitureVMs);
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
