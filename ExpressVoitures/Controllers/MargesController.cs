using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class MargesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MargesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get
        public IActionResult Index()
        {
            var objetmarge = _context.Marges.OrderByDescending(m => m.Id).FirstOrDefault();
            return View(objetmarge);
        }

        [HttpPost]
        public IActionResult Index(Marge marge)
        {
            _context.Add(marge);
            _context.SaveChanges();
            return RedirectToAction("Index", "voitures");
        }
    }
}
