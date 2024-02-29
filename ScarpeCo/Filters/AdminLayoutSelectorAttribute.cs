using System.Web.Mvc;

namespace ScarpeCo.Filters
{
    public class AdminLayoutSelectorAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            ViewResult viewResult = filterContext.Result as ViewResult;
            if (viewResult != null) // Assicurati che sia un risultato di tipo View
            {
                // Controlla se l'utente è segnato come admin nella sessione
                bool isAdmin = filterContext.HttpContext.Session["IsAdmin"] != null && (bool)filterContext.HttpContext.Session["IsAdmin"];
                viewResult.MasterName = isAdmin ? "~/Views/Shared/_AdminLayout.cshtml" : "~/Views/Shared/_Layout.cshtml";
            }
        }
    }
}
