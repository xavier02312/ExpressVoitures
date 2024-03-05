using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models
{
    public class Voiture
    {
        public int Id { get; set; }

        [Display(Name = "Code Vin")]
        public string? CodeVin { get; set; }

        public class AnneeValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                int? annee = value as int?;

                if (annee == null || (annee >= 1990 && annee <= DateTime.Now.Year))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("La date d'achat doit être postérieure à 1990 et inférieur ou égale à l'année en cours");
            }
        }
        [Required(ErrorMessage = "l'année de la  voiture doit être complétée")]
        [RegularExpression("^\\d+$", ErrorMessage = "L'année doit être un entier")]
        [AnneeValidation]
        public int Annee { get; set; }

        [Required(ErrorMessage = "La marque doit être complétée")]
        public string? Marque { get; set; }

        [Required(ErrorMessage = "Le modèle doit être complétée")]
        public string? Modele { get; set; }

        public string? Finition { get; set; }


        public class DateAchatValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                DateTime? date = value as DateTime?;

                if (date == null || (date >= new DateTime(1990, 1, 1) && date <= DateTime.Now))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("La date d'achat doit être postérieure à 1990 et inférieur ou égale à la date du jour");
            }
        }
        [Display(Name = "Date Achat")]
        [Required(ErrorMessage = "la date d'achat de la voiture doit être complétée")]
        [DataType(DataType.Date, ErrorMessage = "La date d'achat doit être une date.")]
        [DateAchatValidation]
        public DateTime DateAchat { get; set; }

        [Display(Name = "Prix Achat")]
        [Required(ErrorMessage = "le prix d'achat de la voiture doit être complétée")]
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix d'achat doit être un nombre")]
        public float PrixAchat { get; set; }

        public class DateDisponibiliteDeVenteValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                DateTime? date = value as DateTime?;
                if (validationContext == null)
                {
                    throw new ArgumentNullException(nameof(validationContext));
                }

                var dateAchat = (DateTime)validationContext.ObjectInstance.GetType().GetProperty("DateAchat").GetValue(validationContext.ObjectInstance);

                if (date == null || (date >= dateAchat && date <= DateTime.Now.AddYears(1)))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("La date de disponibilité de vente doit être postérieure ou égale à la date d'achat et inférieur ou égale à la date du jour + 1 an");
            }
        }
        [Display(Name = "Date Disponibilite de Vente")]
        [DataType(DataType.Date, ErrorMessage = "La date de disponibilité de vente doit être une date.")]
        [DateDisponibiliteDeVenteValidation]
        public DateTime? DateDisponibiliteDeVente { get; set; }

        [Display(Name = "Prix de Vente")]
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix de vente doit être un nombre")]
        public float? PrixDeVente { get; set; }

        public class DateDeVenteValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                DateTime? date = value as DateTime?;
                if (validationContext == null)
                {
                    throw new ArgumentNullException(nameof(validationContext));
                }

                if (date == null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    var dateDisponibiliteDeVente = (DateTime)validationContext.ObjectInstance.GetType().GetProperty("DateDisponibiliteDeVente").GetValue(validationContext.ObjectInstance);

                    if (date == null || (date >= dateDisponibiliteDeVente && date <= DateTime.Now))
                    {
                        return ValidationResult.Success;
                    }
                }
                return new ValidationResult("La date de vente doit être postérieure ou égale à la date de disponibilité de la vente et inférieure ou égale à la date du jour");
            }
        }
        [Display(Name = "Date de Vente")]
        [DataType(DataType.Date, ErrorMessage = "La date de vente doit être une date.")]
        [DateDeVenteValidation]
        public DateTime? DateDeVente { get; set; }

        public ICollection<Reparation>? Reparations { get; set; }
    }
}
