using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using ExpressVoitures.Models.Service;
using Microsoft.AspNetCore.Authorization;

namespace ExpressVoitures.Controllers
{
    [Authorize]
    public class AnnoncesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public AnnoncesController(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
        }

        // GET: Annonces
        public async Task<IActionResult> Index(int idVoiture)
        {
            Annonce? annonce = _context?.Annonces?.FirstOrDefault(r => r.IdVoiture == idVoiture);

            if (annonce == null)
            {
                return NotFound();
            }
            else
            {
                annonce.VoitureImages = _context.VoitureImages
                    .Where(a => a.IdAnnonce == annonce.Id)
                    .ToList();

                return View(annonce);
            }

            /*return View(annonce);*/

        }

        // GET: Annonces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (annonce == null)
            {
                return NotFound();
            }

            return View(annonce);
        }

        // GET: Annonces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Annonces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdVoiture,TitreAnnonce,DescriptionAnnonce, VoitureImages")] Annonce annonce, List<IFormFile> VoitureImages, int IdVoiture)
        {
           /* if (ModelState.IsValid)
            {
                if (VoitureImages != null && VoitureImages.Count > 0)
                {
                    foreach (var file in VoitureImages)
                    {
                        if (file != null && file.Length > 0)
                        {
                            // créer un nouvel objet Photo et l'ajouter à la collection Photos de l'annonce
                            var photo = new VoitureImage
                            {
                                Nom = file.FileName,
                                IdAnnonce = annonce.Id
                            };

                            if (annonce.VoitureImages == null)
                            {
                                annonce.VoitureImages = new List<VoitureImage>();
                            }
                            annonce.VoitureImages.Add(photo);

                            _context.Add(annonce);
                            await _context.SaveChangesAsync();
                            var pathService = new PathService(_configuration, _environment);
                            var filePath = pathService.GetUploadsPath(photo.Nom);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                    }
                }
                else
                {
                    // sauvegarder l'annonce dans la base de données sans photo
                    _context.Add(annonce);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }*/
            return View(annonce);

        }

        // GET: Annonces/Edit/5
        public async Task<IActionResult> Edit(int? id, int idVoiture)
        {
            ViewData["idVoiture"] = idVoiture;
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces.FindAsync(id);
            if (annonce == null)
            {
                return NotFound();
            }
            else
            {
                annonce.VoitureImages = _context.VoitureImages
                    .Where(a => a.IdAnnonce == id)
                    .ToList();

                return View(annonce);
            }
        }

        // POST: Annonces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitreAnnonce,DescriptionAnnonce")] Annonce annonce, IFormFile? VoitureImages, int IdVoiture)
        {
            ViewData["idVoiture"] = IdVoiture;
            if (int.TryParse(Request.Form["idVoiture"], out int idVoiture))
                annonce.IdVoiture = idVoiture;

            if (id != annonce.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (VoitureImages != null && VoitureImages.Length > 0)
                    {
                        // créer un nouvel objet Photo et l'ajouter à la collection Photos de l'annonce
                        var voitureImage = new VoitureImage
                        {
                            Nom = VoitureImages.FileName,
                            IdAnnonce = annonce.Id
                        };

                        await _context.SaveChangesAsync();
                        var pathService = new PathService(_configuration, _environment);
                        var filePath = pathService.GetUploadsPath(voitureImage.Nom);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await VoitureImages.CopyToAsync(stream);
                        }

                        if (annonce.VoitureImages == null)
                        {
                            annonce.VoitureImages = new List<VoitureImage>();
                        }
                        voitureImage.LienPhoto = filePath;
                        annonce.VoitureImages.Add(voitureImage);
                        _context.Annonces.Update(annonce);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // sauvegarder l'annonce dans la base de données sans photo
                        _context.Add(annonce);
                        _context.Annonces.Update(annonce);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnonceExists(annonce.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { idVoiture = annonce.IdVoiture });
            }

            return View(annonce);
        }

        // GET: Annonces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (annonce == null)
            {
                return NotFound();
            }

            return View(annonce);
        }

        // POST: Annonces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annonce = await _context.Annonces.FindAsync(id);
            if (annonce != null)
            {
                _context.Annonces.Remove(annonce);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnonceExists(int id)
        {
            return _context.Annonces.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AjouterPhoto(int idAnnonce, IFormFile VoitureImages)
        {
            // récupére l'annonce correspondante à l'ID
            var annonce = await _context.Annonces.FindAsync(idAnnonce);

            if (annonce == null)
            {
                return NotFound();
            }

            // créer un nouvel objet Photo et l'ajouter à la collection Photos de l'annonce
            var voitureImage = new VoitureImage
            {
                Nom = VoitureImages.FileName,
                IdAnnonce = idAnnonce
            };

            if (annonce.VoitureImages == null)
            {
                annonce.VoitureImages = new List<VoitureImage>();
            }
            annonce.VoitureImages.Add(voitureImage);

            // sauvegarder l'annonce dans la base de données
            await _context.SaveChangesAsync();

            // télécharger le fichier dans le répertoire approprié
            var pathService = new PathService(_configuration, _environment);
            var filePath = pathService.GetUploadsPath(voitureImage.Nom);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await VoitureImages.CopyToAsync(stream);
            }

            return RedirectToAction(nameof(Details), new { idAnnonce = idAnnonce });
        }
    }
}
