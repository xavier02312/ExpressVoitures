using ExpressVoitures.Data;
using ExpressVoitures.Models.Service;
using ExpressVoitures.Models;
using static ExpressVoitures.Models.Voiture;

namespace ExpressVoitures.ViewModels
{
    public class VoitureVM
    {
        public ApplicationDbContext _context;

        public Voiture Voiture { get; set; }

        public float TotalReparation;

        public string StatutVoiture;

        public Annonce Annonce { get; set; }

        public VoitureVM(Voiture Voiture, ApplicationDbContext context)
        {
            this.Voiture = Voiture;
            this._context = context;

            this.Annonce = _context.Annonces.FirstOrDefault(a => a.IdVoiture == Voiture.Id);
            Annonce.VoitureImages = _context.VoitureImages
                .Where(a => a.IdAnnonce == Annonce.Id)
                .ToList();
        }

        public float CalculTotalReparation()
        {
            ReparationService reparationService = new ReparationService(_context);
            return TotalReparation = reparationService.SommeReparations(this.Voiture.Id);
        }

        public float CalculPrixVente()
        {
            if (this.DerniereMargeParVoiture != null)
            {
                return this.Voiture.PrixAchat + CalculTotalReparation() + this.DerniereMargeParVoiture();
            }
            else
            {
                return this.Voiture.PrixAchat + CalculTotalReparation();
            }

        }

        public int DerniereMargeParVoiture()
        {
            try
            {
                return _context.Marges.OrderByDescending(m => m.Id).FirstOrDefault().Value;
            }
            catch
            {

                Object objetmarge = null;
                objetmarge = new Marge { Value = 500 };
                _context.Add(objetmarge);
                _context.SaveChanges();
                return _context.Marges.OrderByDescending(m => m.Id).FirstOrDefault().Value;
            }

        }

        public string CalculStatutVoiture()
        {
            if (Voiture.DateDisponibiliteDeVente != null)
            {
                if (Voiture.DateDeVente == null)
                {
                    return StatutVoiture = "Disponible à la vente";
                }
                else
                {
                    return StatutVoiture = "Vendue";
                }
            }
            return StatutVoiture = "en préparation, bientôt à la vente";
        }
    }
}
