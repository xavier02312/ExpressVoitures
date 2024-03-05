using ExpressVoitures.Data;
using ExpressVoitures.Models;
using ExpressVoitures.Models.Service;

namespace ExpressVoitures.ViewModels
{
    public class ReparationsVM
    {
        public ApplicationDbContext _context;
        public int IdVoiture { get; set; }

        public IEnumerable<ReparationavecVoiture>? Reparations { get; set; }

        public string IndexTitle;
        public float TotalReparation;

        public ReparationsVM(int IdVoiture, IEnumerable<ReparationavecVoiture>? Reparations, ApplicationDbContext context)
        {
            this.IdVoiture = IdVoiture;
            this.Reparations = Reparations;
            this._context = context;

            if (Reparations != null && Reparations.Any())
            {
                var DateOnlyAchatVoiture = DateOnly.FromDateTime(Reparations.FirstOrDefault().Reparation.Voiture.DateAchat);

                IndexTitle = "Liste des réparations du véhicule de marque " + Reparations.FirstOrDefault().Reparation.Voiture.Marque +
                    " de modèle " + Reparations.FirstOrDefault().Reparation.Voiture.Modele
                    + " acheté le " + DateOnlyAchatVoiture;
            }
            else
            {
                IndexTitle = "Liste des réparations du véhicule:";
            }
        }

        public float CalculTotalReparation()
        {
            ReparationService reparationService = new ReparationService(_context);
            return TotalReparation = reparationService.SommeReparations(IdVoiture);
        }

    }
    public class ReparationavecVoiture
    {
        public Reparation Reparation { get; set; }

        public Voiture? Voiture { get; set; }
    }
}
