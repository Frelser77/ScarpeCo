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
        [Range(0.01, 10000.00, ErrorMessage = "Il prezzo deve essere tra 0.01 e 10000.00.")]
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


        public List<Articolo> GetAllArticoli()
        {
            List<Articolo> articoli = new List<Articolo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM Articoli";
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
                        articoli.Add(articolo);
                    }
                }
                catch (Exception ex)
                {
                    // Gestisci l'eccezione
                    throw new Exception("Errore durante il recupero degli articoli", ex);
                }
                finally
                {
                    conn.Close();
                }
            }

            return articoli;
        }

        public void AddArticolo(Articolo nuovoArticolo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = "INSERT INTO Articoli (Nome, Prezzo, Descrizione, ImmagineCopertinaUrl, ImmagineAggiuntiva1Url, ImmagineAggiuntiva2Url, InVetrina) VALUES (@Nome, @Prezzo, @Descrizione, @ImmagineCopertinaUrl, @ImmagineAggiuntiva1Url, @ImmagineAggiuntiva2Url, @InVetrina)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@Nome", nuovoArticolo.Nome);
                cmd.Parameters.AddWithValue("@Prezzo", nuovoArticolo.Prezzo);
                cmd.Parameters.AddWithValue("@Descrizione", nuovoArticolo.Descrizione);
                cmd.Parameters.AddWithValue("@ImmagineCopertinaUrl", nuovoArticolo.ImmagineCopertinaUrl);
                cmd.Parameters.AddWithValue("@ImmagineAggiuntiva1Url", nuovoArticolo.ImmagineAggiuntiva1Url);
                cmd.Parameters.AddWithValue("@ImmagineAggiuntiva2Url", nuovoArticolo.ImmagineAggiuntiva2Url);
                cmd.Parameters.AddWithValue("@InVetrina", nuovoArticolo.InVetrina);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Gestisci l'eccezione
                    throw new Exception("Errore durante l'aggiunta di un nuovo articolo", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void UpdateArticolo(Articolo articolo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Articoli SET Nome = @Nome, Prezzo = @Prezzo, Descrizione = @Descrizione, ImmagineCopertinaUrl = @ImmagineCopertinaUrl, ImmagineAggiuntiva1Url = @ImmagineAggiuntiva1Url, ImmagineAggiuntiva2Url = @ImmagineAggiuntiva2Url, InVetrina = @InVetrina WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@Id", articolo.Id);
                cmd.Parameters.AddWithValue("@Nome", articolo.Nome);
                cmd.Parameters.AddWithValue("@Prezzo", articolo.Prezzo);
                cmd.Parameters.AddWithValue("@Descrizione", articolo.Descrizione);
                cmd.Parameters.AddWithValue("@ImmagineCopertinaUrl", articolo.ImmagineCopertinaUrl);
                cmd.Parameters.AddWithValue("@ImmagineAggiuntiva1Url", articolo.ImmagineAggiuntiva1Url);
                cmd.Parameters.AddWithValue("@ImmagineAggiuntiva2Url", articolo.ImmagineAggiuntiva2Url);
                cmd.Parameters.AddWithValue("@InVetrina", articolo.InVetrina);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Gestisci l'eccezione
                    throw new Exception("Errore durante l'aggiornamento dell'articolo", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void ToggleInVetrina(int articoloId, bool nuovoStatoInVetrina)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Articoli SET InVetrina = @InVetrina WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@Id", articoloId);
                cmd.Parameters.AddWithValue("@InVetrina", nuovoStatoInVetrina);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Gestisci l'eccezione
                    throw new Exception("Errore durante l'aggiornamento dello stato InVetrina dell'articolo", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

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