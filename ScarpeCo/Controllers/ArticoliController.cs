using ScarpeCo.Filters;
using ScarpeCo.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ScarpeCo.Controllers
{
    [AdminLayoutSelector]
    public class ArticoliController : Controller
    {
        private readonly Articolo articoloModel = new Articolo();
        private readonly Admin adminModel = new Admin();

        // GET: Articoli
        public ActionResult Index()
        {
            if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
            {
                // Se l'utente è un amministratore, mostra tutti gli articoli
                List<Articolo> tuttiGliArticoli = adminModel.GetAllArticoli();
                return View(tuttiGliArticoli);
            }
            else
            {
                // Se l'utente non è un amministratore, mostra solo gli articoli in vetrina
                List<Articolo> articoliInVetrina = articoloModel.GetArticoliInVetrina();
                return View(articoliInVetrina);
            }
        }

        // GET: Articoli/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id <= 0)
            {
                // Se l'id non è valido, reindirizza l'utente all'elenco degli articoli
                return RedirectToAction("All");
            }

            Articolo articolo = articoloModel.GetArticoliInVetrina().Find(a => a.Id == id);
            if (articolo == null)
            {
                // Se non viene trovato nessun articolo con l'id fornito, reindirizza all'index
                return RedirectToAction("All");
            }

            return View(articolo);
        }
    }
}