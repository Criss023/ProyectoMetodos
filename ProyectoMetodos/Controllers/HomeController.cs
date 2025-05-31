using System.Web.Mvc;

public class HomeController : Controller
{
    public ActionResult Inicio()
    {
        if (Session["usuario"] == null) return RedirectToAction("Index", "Login");
        return View();
    }

    public ActionResult Integrantes()
    {
        if (Session["usuario"] == null) return RedirectToAction("Index", "Login");
        return View();
    }

    public ActionResult AcercaDe()
    {
        if (Session["usuario"] == null) return RedirectToAction("Index", "Login");
        return View();
    }
}
