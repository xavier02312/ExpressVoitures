using ExpressVoitures.Data;
using ExpressVoitures.ViewModels;

namespace ExpressVoitures.Models.Service
{
    public class ReparationService : IReparationService
    {
        private readonly ApplicationDbContext _context;

        public ReparationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public float SommeReparations(int idVoiture)
        {
            float Somme = 0;

            List<ReparationavecVoiture> ListeReparationsVehicule = _context.Reparations
                .Where(r => r.IdVoiture == idVoiture)
                .Select(r => new ReparationavecVoiture { Reparation = r, Voiture = r.Voiture })
                .ToList();


            if (ListeReparationsVehicule != null)
            {
                foreach (var r in ListeReparationsVehicule)
                {
                    Somme += r.Reparation.CoutsReparation;
                }
            }
            return Somme;
        }
    }
}
