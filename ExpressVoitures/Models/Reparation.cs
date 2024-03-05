using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models
{
    public class Reparation
    {
        public int Id { get; set; }

        public int IdVoiture { get; set; }

        [Display(Name = "Type de Réparation")]
        public string? TypeDeReparation { get; set; }

        [Display(Name = "Coût Réparation")]
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix doit être un nombre")]
        public float CoutsReparation { get; set; }

        [NotMapped]
        public Voiture? Voiture { get; set; }
    }
}
