using System.ComponentModel.DataAnnotations;

namespace ScarpeCo.Models
{
    public class Utente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(60, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        [StringLength(60, ErrorMessage = "Il cognome non può superare i 100 caratteri")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "L'età è obbligatoria")]
        [Range(1, 120, ErrorMessage = "L'età deve essere compresa tra 1 e 120")]
        [Display(Name = "Età")]
        public int Eta { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        [StringLength(50, ErrorMessage = "L'email non può superare i 30 caratteri")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lo username è obbligatorio")]
        [StringLength(50, ErrorMessage = "Lo username non può superare i 50 caratteri")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria")]
        [DataType(DataType.Password)]
        [StringLength(18, ErrorMessage = "La password puó essere massimo di 18 caratteri")]
        public string Password { get; set; }

        [Required]
        public bool Admin { get; set; }

    }
}