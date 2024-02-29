using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ScarpeCo.Models
{
    public class Admin : Utente
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;

        public static List<Utente> GetAllUtenti()
        {
            List<Utente> utenti = new List<Utente>();
            string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Utenti";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Utente utente = new Utente
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                Eta = (int)reader["Eta"],
                                Email = reader["Email"].ToString(),
                                Username = reader["Username"].ToString(),
                                Admin = (bool)reader["Admin"]
                            };
                            utenti.Add(utente);
                        }
                    }
                }
            }
            return utenti;
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


        public Articolo CloneArticolo(int articoloId)
        {
            Articolo articoloOriginale = GetAllArticoli().Find(a => a.Id == articoloId);
            if (articoloOriginale == null) return null; // Se l'articolo non esiste, ritorna null

            Articolo articoloClonato = new Articolo
            {
                Nome = articoloOriginale.Nome + "-clone",
                Prezzo = articoloOriginale.Prezzo,
                Descrizione = articoloOriginale.Descrizione,
                ImmagineCopertinaUrl = articoloOriginale.ImmagineCopertinaUrl,
                ImmagineAggiuntiva1Url = articoloOriginale.ImmagineAggiuntiva1Url,
                ImmagineAggiuntiva2Url = articoloOriginale.ImmagineAggiuntiva2Url,
                InVetrina = false
            };

            AddArticolo(articoloClonato);

            return articoloClonato;
        }

        // Metodo per eliminare un articolo
        public bool DeleteArticolo(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Verifica esistenza dell'articolo
                if (!Exists(id))
                {
                    return false; // Articolo non trovato
                }

                string sqlQuery = "DELETE FROM Articoli WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Ritorna true se l'eliminazione ha avuto successo
                }
                catch (Exception ex)
                {
                    // Log dell'errore
                    throw new Exception("Errore durante l'eliminazione dell'articolo", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private bool Exists(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT COUNT(*) FROM Articoli WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    // ExecuteScalar restituisce il valore della prima colonna della prima riga nel set di risultati
                    // restituirà il conteggio degli articoli che corrispondono all'ID
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Errore durante la verifica dell'esistenza dell'articolo", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}