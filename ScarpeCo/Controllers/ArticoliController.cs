using ScarpeCo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ScarpeCo.Controllers
{
    public class ArticoliController : Controller
    {
        private readonly Articolo articoloModel = new Articolo();

        // GET: Articoli
        public ActionResult Index()
        {
            List<Articolo> articoliInVetrina = articoloModel.GetArticoliInVetrina();
            return View(articoliInVetrina);
        }

        // GET: Articoli/All 
        // Questo metodo è stato aggiunto per mostrare tutti gli articoli, non solo quelli in vetrina 
        // in un secondo momento sará visibile solo a chi ha i permessi di amministratore
        public ActionResult All()
        {
            List<Articolo> tuttiGliArticoli = articoloModel.GetAllArticoli();
            return View(tuttiGliArticoli);
        }

        // GET: Articoli/Details/5
        public ActionResult Details(int id)
        {
            Articolo articolo = articoloModel.GetAllArticoli().Find(a => a.Id == id);
            return View(articolo);
        }

        // GET: Articoli/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "ImmagineCopertinaUrl,ImmagineAggiuntiva1Url,ImmagineAggiuntiva2Url")] Articolo articolo)
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
                    // Assicurati che AddArticolo accetti i percorsi delle immagini come stringhe e non gli oggetti HttpPostedFileBase
                    articoloModel.AddArticolo(articolo);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    // Gestione dell'errore
                    ModelState.AddModelError("", "Si è verificato un errore durante il salvataggio dell'articolo.");
                }
            }

            return View(articolo);
        }



        // GET: Articoli/Edit/5
        public ActionResult Edit(int id)
        {
            Articolo articolo = articoloModel.GetAllArticoli().Find(a => a.Id == id);
            return View(articolo);
        }

        // POST: Articoli/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Articolo articolo)
        {
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

        // GET: Articoli/ToggleInVetrina/5
        public ActionResult ToggleInVetrina(int id)
        {
            Articolo articolo = articoloModel.GetAllArticoli().Find(a => a.Id == id);
            if (articolo != null)
            {
                articoloModel.ToggleInVetrina(articolo.Id, !articolo.InVetrina);
            }
            return RedirectToAction("All");
        }
    }
}