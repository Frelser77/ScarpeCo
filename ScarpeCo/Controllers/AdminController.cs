using ScarpeCo.Filters;
using ScarpeCo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace ScarpeCo.Controllers
{
    [AdminLayoutSelector]
    public class AdminController : Controller
    {
        private readonly Admin articoloModel = new Admin();
        // CONTROLLI UTENTE
        // GET: Admin
        public ActionResult Index()
        {

            if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
            {
                List<Utente> utenti = Admin.GetAllUtenti();
                return View(utenti);
            }
            return RedirectToAction("Login");
        }

        // GET: Utente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id <= 0)
            {
                // Se l'id non è valido, reindirizza l'utente all'elenco degli utenti
                return RedirectToAction("Index");
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;
            Utente utente = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Utenti WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            utente = new Utente
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                Eta = (int)reader["Eta"],
                                Email = reader["Email"].ToString(),
                                Username = reader["Username"].ToString(),
                                Admin = (bool)reader["Admin"]
                            };
                        }
                    }
                }
            }

            if (utente == null)
            {
                // Se non viene trovato nessun utente con l'id fornito, reindirizza all'index
                return RedirectToAction("Index");
            }

            return View(utente);
        }

        // GET: Utente/Edit/5
        // Mostra il form per la modifica di un utente
        public ActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                // Se l'id non è valido, reindirizza l'utente all'elenco degli utenti
                return RedirectToAction("Index");
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;
            Utente utente = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Utenti WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            utente = new Utente
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                Eta = (int)reader["Eta"],
                                Email = reader["Email"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Admin = (bool)reader["Admin"]
                            };
                        }
                    }
                }
            }
            if (utente == null)
            {
                return RedirectToAction("Index");
            }
            return View(utente);
        }

        // POST: Utente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Utente utente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Utenti SET Nome = @Nome, Cognome = @Cognome, Eta = @Eta, Email = @Email, Username = @Username, Password = @Password, Admin = @Admin WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", utente.Id);
                        cmd.Parameters.AddWithValue("@Nome", utente.Nome);
                        cmd.Parameters.AddWithValue("@Cognome", utente.Cognome);
                        cmd.Parameters.AddWithValue("@Eta", utente.Eta);
                        cmd.Parameters.AddWithValue("@Email", utente.Email);
                        cmd.Parameters.AddWithValue("@Username", utente.Username);
                        cmd.Parameters.AddWithValue("@Password", utente.Password);
                        cmd.Parameters.AddWithValue("@Admin", utente.Admin);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Modifica dati utente completata con successo.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Errore durante la modifica dati utente.";
            return View(utente);
        }

        // POST: Utente/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Errore: Utente non valido.";
                return RedirectToAction("Index");
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Utenti WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            TempData["SuccessMessage"] = "Utente eliminato con successo.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Errore: Utente non trovato.";
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Gestisci l'eccezione, ad esempio loggando l'errore
                TempData["ErrorMessage"] = "Errore durante la cancellazione dell'utente.";
            }

            return RedirectToAction("Index");
        }

        // CONTROLLI ARTICOLI
        // GET: Articoli/All 
        // Questo metodo è stato aggiunto per mostrare tutti gli articoli, non solo quelli in vetrina 
        public ActionResult All()
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
            {
                return RedirectToAction("All", "Admin");
            }
            List<Articolo> tuttiGliArticoli = articoloModel.GetAllArticoli();
            return View(tuttiGliArticoli);
        }



        // GET: Articoli/Create
        public ActionResult ArticoloCreate()
        {
            Articolo model = new Articolo();
            return View(model);
        }

        [HttpPost]
        public ActionResult ArticoloCreate([Bind(Exclude = "ImmagineCopertinaUrl,ImmagineAggiuntiva1Url,ImmagineAggiuntiva2Url")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {

                if (articolo.FileImmagineCopertina != null && articolo.FileImmagineCopertina.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(articolo.FileImmagineCopertina.FileName);
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles/"), fileName);
                    articolo.FileImmagineCopertina.SaveAs(path);
                    articolo.ImmagineCopertinaUrl = "/UploadedFiles/" + fileName;
                }

                if (articolo.FileImmagineAggiuntiva1 != null && articolo.FileImmagineAggiuntiva1.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(articolo.FileImmagineAggiuntiva1.FileName);
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles/"), fileName);
                    articolo.FileImmagineAggiuntiva1.SaveAs(path);
                    articolo.ImmagineAggiuntiva1Url = "/UploadedFiles/" + fileName;
                }

                if (articolo.FileImmagineAggiuntiva2 != null && articolo.FileImmagineAggiuntiva2.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(articolo.FileImmagineAggiuntiva2.FileName);
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles/"), fileName);
                    articolo.FileImmagineAggiuntiva2.SaveAs(path);
                    articolo.ImmagineAggiuntiva2Url = "/UploadedFiles/" + fileName;
                }

                try
                {
                    articoloModel.AddArticolo(articolo);
                    TempData["SuccessMessage"] = "Prodotto creato con successo!";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Si è verificato un errore durante il salvataggio dell'articolo.");
                }
            }

            return View(articolo);
        }

        // GET: Articoli/Edit/5
        public ActionResult ArticoloEdit(int? id)
        {
            if (id == null || id <= 0)
            {
                // Se l'id non è valido, reindirizza l'utente all'elenco degli articoli
                return RedirectToAction("Index");
            }

            Articolo articolo = articoloModel.GetAllArticoli().Find(a => a.Id == id);
            if (articolo == null)
            {
                // Se non viene trovato nessun articolo con l'id fornito, reindirizza all'index
                TempData["ErrorMessage"] = "Articolo non trovato o errore nella modifica.";
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Prodotto modificato con successo!";

            return View(articolo);
        }

        // POST: Articoli/Edit/5
        [HttpPost]
        public ActionResult ArticoloEdit(int? id, Articolo articolo)
        {
            if (id == null || id <= 0)
            {
                // Se l'id non è valido, reindirizza l'utente all'elenco degli articoli
                return RedirectToAction("Index");
            }
            try
            {
                articoloModel.UpdateArticolo(articolo);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Articoli/ToggleInVetrina/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleInVetrina(int id)
        {
            Articolo articolo = articoloModel.GetAllArticoli().Find(a => a.Id == id);
            if (articolo == null)
            {
                TempData["ErrorMessage"] = "Articolo non trovato.";
                return RedirectToAction("All");
            }
            else
            {
                // Assumendo che ToggleInVetrina sia un metodo void e non restituisca un valore
                articoloModel.ToggleInVetrina(articolo.Id, !articolo.InVetrina);

                // Aggiungi qui ulteriori controlli se necessario per confermare che l'operazione ha avuto successo

                // Se tutto è andato bene
                TempData["SuccessMessage"] = "Visibilità articolo modificata con successo.";
            }

            return RedirectToAction("All");
        }

        // GET: Articoli/Clone/5
        public ActionResult Clone(int id)
        {

            try
            {
                Articolo articoloClonato = articoloModel.CloneArticolo(id);
                if (articoloClonato == null)
                {
                    TempData["ErrorMessage"] = "Articolo non trovato o errore nella clonazione.";
                    return RedirectToAction("Index");
                }
                TempData["SuccessMessage"] = "Prodotto clonato con successo!";
                return RedirectToAction("Edit", new { id = articoloClonato.Id });
            }
            catch (Exception)
            {
                return RedirectToAction("All");
            }
        }

        // POST: Articoli/Delete/5
        [HttpPost, ActionName("DeleteArticolo")]
        [ValidateAntiForgeryToken]
        public ActionResult ArticoloDelete(int id)
        {
            try
            {
                bool result = articoloModel.DeleteArticolo(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Articolo eliminato con successo.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Articolo non trovato o errore nella cancellazione.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Si è verificato un errore: " + ex.Message;
            }
            return RedirectToAction("All");
        }
    }
}