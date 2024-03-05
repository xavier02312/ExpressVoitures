using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models
{
    public class VoitureImage
    {
        public int Id { get; set; }

        public required int IdAnnonce { get; set; }

        public string? Nom { get; set; }

        public string? LienPhoto { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? Fichier { get; set; }

        [NotMapped]
        public virtual Annonce? _Annonce { get; set; }
    }
}
