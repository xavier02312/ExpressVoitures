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

namespace ExpressVoitures.Controllers
{
    public class VoituresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoituresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Voitures
        public async Task<IActionResult> Index()
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

        // GET: Voitures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _context.Voitures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voiture == null)
            {
                return NotFound();
            }

            return View(voiture);
        }

        // GET: Voitures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voitures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodeVin,Annee,Marque,Modele,Finition,DateAchat,PrixAchat,DateDisponibiliteDeVente,PrixDeVente,DateDeVente")] Voiture voiture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voiture);
                await _context.SaveChangesAsync();

                //création d'une annonce en même temps que la création de la voiture
                Annonce AnnonceCree = new Annonce
                {
                    IdVoiture = voiture.Id,
                    TitreAnnonce = "Vente d'une voiture de marque " + voiture.Marque + " et de modèle "
                           + voiture.Modele + " de " + voiture.Annee
                };
                _context.Add(AnnonceCree);
                await _context.SaveChangesAsync();

                if (voiture.DateDeVente != null)
                {
                    VoitureVM voitureVM = new VoitureVM(voiture, _context);
                    voiture.PrixDeVente = voitureVM.CalculPrixVente();
                    _context.Voitures.Update(voiture);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(voiture);
        }

        // GET: Voitures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _context.Voitures.FindAsync(id);
            if (voiture == null)
            {
                return NotFound();
            }
            return View(voiture);
        }

        // POST: Voitures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodeVin,Annee,Marque,Modele,Finition,DateAchat,PrixAchat,DateDisponibiliteDeVente,PrixDeVente,DateDeVente")] Voiture voiture)
        {
            if (id != voiture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (voiture.PrixDeVente == null && voiture.DateDeVente != null)
                    {
                        VoitureVM voitureVM = new VoitureVM(voiture, _context);
                        voiture.PrixDeVente = voitureVM.CalculPrixVente();
                        _context.Voitures.Update(voiture);
                        await _context.SaveChangesAsync();
                    }

                    _context.Update(voiture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoitureExists(voiture.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(voiture);
        }

        // GET: Voitures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _context.Voitures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voiture == null)
            {
                return NotFound();
            }

            return View(voiture);
        }

        // POST: Voitures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voiture = await _context.Voitures.FindAsync(id);
            var annonce = await _context.Annonces
                .SingleOrDefaultAsync(a => a.IdVoiture == id);

            List<VoitureImage> voitureImages = await _context.VoitureImages.Where(p => p.IdAnnonce == annonce.Id).ToListAsync();


            if (voitureImages != null)
            {
                foreach (VoitureImage voitureImage in voitureImages)
                {
                    _context.VoitureImages.Remove(voitureImage);
                }
            }

            if (voiture != null && annonce != null)
            {
                _context.Annonces.Remove(annonce);
                _context.Voitures.Remove(voiture);
            }

            else if (voiture != null)
            {
                _context.Voitures.Remove(voiture);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoitureExists(int id)
        {
            return _context.Voitures.Any(e => e.Id == id);
        }
    }
}
