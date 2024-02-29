using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace ScarpeCo.Models
{
    public class Articolo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome dell'articolo è obbligatorio.")]
        [StringLength(255, ErrorMessage = "Il nome non può superare i 255 caratteri.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il prezzo dell'articolo è obbligatorio.")]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal Prezzo { get; set; }

        [Required(ErrorMessage = "La descrizione dell'articolo è obbligatoria.")]
        public string Descrizione { get; set; }

        [Display(Name = "Carica l'immagine di copertina")]
        public string ImmagineCopertinaUrl { get; set; }

        [Display(Name = "Carica un immagine aggiuntiva")]
        public string ImmagineAggiuntiva1Url { get; set; }

        [Display(Name = "Carica un'altra immagine aggiuntiva")]
        public string ImmagineAggiuntiva2Url { get; set; }

        public HttpPostedFileBase FileImmagineCopertina { get; set; }
        public HttpPostedFileBase FileImmagineAggiuntiva1 { get; set; }
        public HttpPostedFileBase FileImmagineAggiuntiva2 { get; set; }

        [Required]
        public bool InVetrina { get; set; }


        private readonly string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;

        public List<Articolo> GetArticoliInVetrina()
        {
            List<Articolo> articoliInVetrina = new List<Articolo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM Articoli WHERE InVetrina = 1";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Articolo articolo = new Articolo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Prezzo = Convert.ToDecimal(reader["Prezzo"]),
                            Descrizione = reader["Descrizione"].ToString(),
                            ImmagineCopertinaUrl = reader["ImmagineCopertinaUrl"].ToString(),
                            ImmagineAggiuntiva1Url = reader["ImmagineAggiuntiva1Url"].ToString(),
                            ImmagineAggiuntiva2Url = reader["ImmagineAggiuntiva2Url"].ToString(),
                            InVetrina = Convert.ToBoolean(reader["InVetrina"])
                        };
                        articoliInVetrina.Add(articolo);
                    }
                }
                catch (Exception ex)
                {
                    // Gestisci l'eccezione
                    throw new Exception("Errore durante il recupero degli articoli in vetrina", ex);
                }
                finally
                {
                    conn.Close();
                }
            }

            return articoliInVetrina;
        }

    }
}