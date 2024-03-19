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

        // Date achat postérieur à l'annnée
        public class DateAchatValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                DateTime? date = value as DateTime?;
                
                var voiture = (Voiture)validationContext.ObjectInstance;

                if (date == null || (date >= new DateTime(1990, 1, 1) && date <= DateTime.Now) && date.Value.Year >= voiture.Annee)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("La date d'achat doit être postérieure à l'Année");
            }
        }
        [Display(Name = "Date Achat")]
        [Required(ErrorMessage = "la date d'achat de la voiture doit être complétée")]
        [DataType(DataType.Date, ErrorMessage = "La date d'achat doit être une date.")]
        [DateAchatValidation]
        public DateTime DateAchat { get; set; }

        // Le prix d'achat doit être de 500 €
        public class PrixAchatValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                float? PrixAchat = value as float?;

                if (PrixAchat != null && (PrixAchat >= 500))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("Le prix d'achat doit être de 500 € minimum");
            }
        }
       
        [Display(Name = "Prix Achat")]
        [Required(ErrorMessage = "le prix d'achat de la voiture doit être complétée")]
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix d'achat doit être un nombre")]
        [PrixAchatValidation]
        public float PrixAchat { get; set; }

        // DateDisponibiliteDeVente doit être postérieure à la date d'Achat et Annee//
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

        // Prix de vente doit être supérieur au PrixAchat + Marge
        public class PrixDeVenteValidationAttribute : ValidationAttribute
        {
            private readonly float _marge;

            public PrixDeVenteValidationAttribute(float marge)
            {
                _marge = marge;
            }
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                float? PrixDeVente = value as float?;
                var voiture = (Voiture)validationContext.ObjectInstance;

                if (PrixDeVente == null)
                {
                    return new ValidationResult("Le prix de vente doit être complété avec le prix d'achat + marge");
                }
                else if (PrixDeVente >= voiture.PrixAchat + _marge)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Il n'est pas possible de vendre une voiture en dessous du prix d'achat + la marge");
                }
            }
        }

        [Display(Name = "Prix de Vente")]
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix de vente doit être un nombre")]
        // Marge defini minimum 500 €
        [PrixDeVenteValidation(500)]
        public float? PrixDeVente { get; set; }

        // Date de Vente  
        public class DateDeVenteValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                DateTime? dateDeVente = value as DateTime?;
                if (validationContext == null)
                {
                    throw new ArgumentNullException(nameof(validationContext));
                }

                var dateDisponibiliteDeVente = (DateTime?)validationContext.ObjectInstance.GetType().GetProperty("DateDisponibiliteDeVente").GetValue(validationContext.ObjectInstance);

                if (dateDeVente != null && dateDisponibiliteDeVente == null)
                {
                    return new ValidationResult("Veuillez remplir Date disponibilité de Vente");
                }
                else if (dateDeVente == null || (dateDeVente >= dateDisponibiliteDeVente && dateDeVente <= DateTime.Now))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("La date de vente doit être supérieure à la date de disponibilité de vente et inférieure ou égale à la date du jour");
            }
        }

        [Display(Name = "Date de Vente")]
        [DataType(DataType.Date, ErrorMessage = "La date de vente doit être une date.")]
        [DateDeVenteValidation]
        public DateTime? DateDeVente { get; set; }

        public ICollection<Reparation>? Reparations { get; set; }
    }
}
