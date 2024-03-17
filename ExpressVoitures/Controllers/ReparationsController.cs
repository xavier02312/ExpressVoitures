using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using ExpressVoitures.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ExpressVoitures.Controllers
{
    [Authorize]
    public class ReparationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReparationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reparations
        public async Task<IActionResult> Index(int idVoiture)
        {
            List<ReparationavecVoiture> reparations = _context.Reparations
                .Where(r => r.IdVoiture == idVoiture)
                .Select(r => new ReparationavecVoiture { Reparation = r, Voiture = r.Voiture })
                .ToList();

            foreach (var r in reparations)
            {
                r.Reparation.Voiture = _context.Voitures.FirstOrDefault(Voiture => Voiture.Id == r.Reparation.IdVoiture);
            }

            ReparationsVM model = new(idVoiture, reparations, _context)
            {
                IdVoiture = idVoiture,
                Reparations = reparations,
                _context = _context
            };

            return View(model);
        }

        // GET: Reparations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reparation = await _context.Reparations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reparation == null)
            {
                return NotFound();
            }

            return View(reparation);
        }

        // GET: Reparations/Create
        public IActionResult Create(int idVoiture)
        {
            ViewData["idVoiture"] = idVoiture;
            return View();
        }

        // POST: Reparations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeDeReparation,CoutsReparation")] Reparation reparation)
        {
            if (int.TryParse(Request.Form["idVoiture"], out int idVoiture))
            {
                reparation.IdVoiture = idVoiture;

                if (ModelState.IsValid)
                {
                    // Ajoutez la réparation à la base de données
                    _context.Add(reparation);
                    await _context.SaveChangesAsync();

                    // Redirigez vers la vue Index
                    return RedirectToAction(nameof(Index), new { idVoiture = idVoiture });
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
            return View(reparation);
        }

        // GET: Reparations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reparation = await _context.Reparations.FindAsync(id);
            if (reparation == null)
            {
                return NotFound();
            }
            return View(reparation);
        }

        // POST: Reparations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeDeReparation,CoutsReparation")] Reparation reparation)
        {
            if (int.TryParse(Request.Form["idVoiture"], out int idVoiture))
                reparation.IdVoiture = idVoiture;

            if (id != reparation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reparation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReparationExists(reparation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return RedirectToAction(nameof(Index), new { idVoiture = reparation.IdVoiture });
        }

        // GET: Reparations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reparation = await _context.Reparations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reparation == null)
            {
                return NotFound();
            }

            return View(reparation);
        }

        // POST: Reparations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reparation = await _context.Reparations.FindAsync(id);
            if (reparation != null)
            {
                _context.Reparations.Remove(reparation);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { idVoiture = reparation.IdVoiture });
        }

        private bool ReparationExists(int id)
        {
            return _context.Reparations.Any(e => e.Id == id);
        }
    }
}
