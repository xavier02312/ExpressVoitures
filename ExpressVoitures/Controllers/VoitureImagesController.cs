using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using ExpressVoitures.Models.Service;

namespace ExpressVoitures.Controllers
{
    public class VoitureImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public VoitureImagesController(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
        }

        // GET: VoitureImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.VoitureImages.ToListAsync());
        }

        // GET: VoitureImages/Details/5
        public async Task<IActionResult> Details(int? id, int idVoiture)
        {
            ViewData["idVoiture"] = idVoiture;
            if (id == null)
            {
                return NotFound();
            }

            var voitureImage = await _context.VoitureImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voitureImage == null)
            {
                return NotFound();
            }

            return View(voitureImage);
        }

        // GET: VoitureImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VoitureImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAnnonce,Nom,LienPhoto")] VoitureImage voitureImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voitureImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voitureImage);
        }

        // GET: VoitureImages/Edit/5
        public async Task<IActionResult> Edit(int? id, int idVoiture, int idAnnonce)
        {
            ViewData["idVoiture"] = idVoiture;
            ViewData["idAnnonce"] = idVoiture;
            if (id == null)
            {
                return NotFound();
            }

            var voitureImage = await _context.VoitureImages.FindAsync(id);
            if (voitureImage == null)
            {
                return NotFound();
            }
            return View(voitureImage);
        }

        // POST: VoitureImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAnnonce,Nom,LienPhoto")] VoitureImage voitureImage)
        {
            /* if (int.TryParse(Request.Form["idAnnonce"], out int idAnnonce))
                photo.IdAnnonce = idAnnonce;*/

            if (id != voitureImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voitureImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoitureImageExists(voitureImage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (int.TryParse(Request.Form["idVoiture"], out int idVoiture))
                {
                    return RedirectToAction("Edit", "Annonces", new { idVoiture = idVoiture, id = voitureImage.IdAnnonce });
                }
            }
            return View(voitureImage);
        }

        // GET: VoitureImages/Delete/5
        public async Task<IActionResult> Delete(int? id, int idVoiture)
        {
            ViewData["idVoiture"] = idVoiture;
            if (id == null)
            {
                return NotFound();
            }

            var voitureImage = await _context.VoitureImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voitureImage == null)
            {
                return NotFound();
            }

            return View(voitureImage);
        }

        // POST: VoitureImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idVoiture)
        {
            var voitureImage = await _context.VoitureImages.FindAsync(id);
            if (voitureImage != null)
            {
                return NotFound();
            }

            //suppresion de la photo stocké
            var pathService = new PathService(_configuration, _environment);
            var filePath = pathService.GetUploadsPath(voitureImage.Nom);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.VoitureImages.Remove(voitureImage);

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "Annonces", new { id = voitureImage.IdAnnonce, idVoiture = idVoiture});
        }

        private bool VoitureImageExists(int id)
        {
            return _context.VoitureImages.Any(e => e.Id == id);
        }
    }
}
