using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models
{
    public class Annonce
    {
        public int Id { get; set; }

        public required int IdVoiture { get; set; }

        [Required]
        [Display(Name = "Titre Annonce")]
        public string? TitreAnnonce { get; set; }

        [Display(Name = "Description Annonce")]
        public string? DescriptionAnnonce { get; set; }

        [Display(Name = "photos")]
        public ICollection<VoitureImage>? VoitureImages { get; set; }

        [NotMapped]
        public virtual Voiture? _Voiture { get; set; }
    }
}
