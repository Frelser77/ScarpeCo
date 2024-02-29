using ScarpeCo.Filters;
using ScarpeCo.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace ScarpeCo.Controllers
{
    [AdminLayoutSelector]
    public class UtenteController : Controller
    {
        private readonly Utente articoloModel = new Utente();

        // GET: Utente/Registrazione
        // Mostra il form di registrazione
        public ActionResult Registrazione()
        {
            return View();
        }

        // GET: Utente/Registrazione
        // metodo per la registrazione di un nuovo utente 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrazione(Utente utente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Utenti (Nome, Cognome, Eta, Email, Username, Password, Admin) VALUES (@Nome, @Cognome, @Eta, @Email, @Username, @Password, @Admin)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {


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
                TempData["SuccessMessage"] = "Registrazione completata con successo. Ora puoi effettuare il login.";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["ErrorMessage"] = "Errore durante la registrazione. Per favore, riprova.";
                return View(utente);
            }
        }

        // GET: Utente/Login
        // Mostra il form di login
        public ActionResult Login()
        {
            // Verifica se l'utente è già loggato e, in caso affermativo, reindirizzalo
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Articoli");
            }
            return View();
        }

        // POST: Utente/Login
        // Processa le credenziali di login inviate dall'utente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Prima controlla se l'username esiste
                    string queryUserExists = "SELECT COUNT(*) FROM Utenti WHERE Username = @Username";
                    SqlCommand cmdUserExists = new SqlCommand(queryUserExists, conn);
                    cmdUserExists.Parameters.AddWithValue("@Username", model.Username);

                    conn.Open();
                    int userExists = (int)cmdUserExists.ExecuteScalar();

                    if (userExists > 0)
                    {
                        // Se l'username esiste, controlla se la password è corretta
                        string queryPasswordCorrect = "SELECT * FROM Utenti WHERE Username = @Username AND Password = @Password";
                        SqlCommand cmdPasswordCorrect = new SqlCommand(queryPasswordCorrect, conn);
                        cmdPasswordCorrect.Parameters.AddWithValue("@Username", model.Username);
                        cmdPasswordCorrect.Parameters.AddWithValue("@Password", model.Password);

                        using (SqlDataReader reader = cmdPasswordCorrect.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Session["UserId"] = reader["Id"].ToString();
                                Session["IsAdmin"] = reader["Admin"];
                                bool isAdmin = Convert.ToBoolean(reader["Admin"]);
                                return isAdmin ? RedirectToAction("All", "Admin") : RedirectToAction("Index", "Articoli");
                            }
                            else
                            {
                                // Username esiste ma la password è sbagliata
                                ModelState.AddModelError("", "La password inserita è sbagliata.");
                                return View(model);
                            }
                        }
                    }
                    else
                    {
                        // Username non esiste
                        ModelState.AddModelError("", "L'username inserito non esiste.");
                        return View(model);
                    }
                }
            }
            // Se arrivi qui, qualcosa è fallito, ri-mostra il form
            return View(model);
        }



        // GET: Utente/Logout
        // Effettua il logout dell'utente
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon(); // Cancella la sessione
            return RedirectToAction("Login", "Utente");
        }


    }
}