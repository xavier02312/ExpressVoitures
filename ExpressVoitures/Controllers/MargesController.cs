using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    [Authorize]
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

        // GET: Marges/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marge = _context.Marges.Find(id);
            if (marge == null)
            {
                return NotFound();
            }
            return View(marge);
        }

        // POST: Marges/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Value")] Marge marge)
        {
            if (id != marge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Vérifiez si la valeur de la marge est inférieure à 500
                    if (marge.Value < 500)
                    {
                        ModelState.AddModelError("Value", "La marge minimale est de 500 €.");
                    }
                    else
                    {
                        // Valide le changement
                        _context.Update(marge);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MargeExists(marge.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(marge);
        }

        private bool MargeExists(int id)
        {
            return _context.Marges.Any(e => e.Id == id);
        }
    }
}
