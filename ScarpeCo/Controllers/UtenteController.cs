using ScarpeCo.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace ScarpeCo.Controllers
{
    public class UtenteController : Controller
    {
        // GET: Utente
        public ActionResult Index()
        {
            if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
            {
                List<Utente> utenti = GetAllUtenti();
                return View(utenti);
            }
            return RedirectToAction("Login");
        }

        private List<Utente> GetAllUtenti()
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

        // GET: Utente/Details/5
        public ActionResult Details(int id)
        {
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
                return HttpNotFound();
            }
            return View(utente);
        }


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

                return RedirectToAction("Login");
            }

            return View(utente);
        }

        // GET: Utente/Login
        // Mostra il form di login
        public ActionResult Login()
        {
            // Verifica se l'utente è già loggato e, in caso affermativo, reindirizzalo
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Utente/Login
        // Processa le credenziali di login inviate dall'utente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Utenti WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", Password);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Imposta l'utente come loggato
                        Session["UserId"] = reader["Id"].ToString();
                        Session["IsAdmin"] = reader["Admin"];
                        if ((bool)Session["IsAdmin"])
                        {
                            return RedirectToAction("Index", "Utente");
                        }
                        return RedirectToAction("Index", "Articoli");
                    }
                }
            }

            // Se le credenziali non sono valide, mostra un messaggio di errore
            ViewBag.ErrorMessage = "Username o password non validi.";
            return View();
        }

        // GET: Utente/Logout
        // Effettua il logout dell'utente
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon(); // Cancella la sessione
            return RedirectToAction("Login", "Utente");
        }

        // GET: Utente/Edit/5
        // Mostra il form per la modifica di un utente
        public ActionResult Edit(int id)
        {
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
                return HttpNotFound();
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
                return RedirectToAction("Index");
            }
            return View(utente);
        }

        // GET: Utente/Delete/5
        // Mostra il form di conferma eliminazione
        public ActionResult Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ScarpeCo"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Utenti WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }
    }
}