using ProyectoMetodos.Models;
using ProyectoMetodos.Utilidades;
using System.Linq;
using System.Web.Mvc;

namespace ProyectoMetodos.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // Iniciar sesion
        [HttpPost]
        public ActionResult Index(string Usuario, string Contrasena)
        {
            // 1) Hashear la contraseña que ingresó
            string contrasenaHasheada = Seguridad.HashearContrasena(Contrasena);

            // 2) Validar con el hash
            UsuarioDB usuarioDB = new UsuarioDB();
            User usuario = usuarioDB.ValidarUsuario(Usuario, contrasenaHasheada);

            if (usuario != null)
            {
                Session["usuario"] = usuario;
                return RedirectToAction("Inicio", "Home"); 
            }

            ViewBag.Error = "Usuario no encontrado";
            return View(); 
        }

        // Cerrar sesion
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear(); 
            return RedirectToAction("Index", "Login");
        }

        UsuarioDB usuarioDB = new UsuarioDB();

        // GET: Login/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Login/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string usuario, string contrasena)
        {
            // Validaciones manuales
            if (string.IsNullOrWhiteSpace(usuario))
                ModelState.AddModelError("usuario", "El usuario es obligatorio");
            if (string.IsNullOrWhiteSpace(contrasena))
                ModelState.AddModelError("contrasena", "La contraseña es obligatoria");

            if (!ModelState.IsValid)
            {
                ViewBag.Usuario = usuario;
                return View();
            }

            var nuevo = new User
            {
                Usuario = usuario,
                Contrasena = Seguridad.HashearContrasena(contrasena)
            };

            bool exito = usuarioDB.RegistrarUsuario(nuevo);
            if (exito)
            {
                TempData["Mensaje"] = "Usuario registrado exitosamente";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ModelState.AddModelError("", "Ya existe un usuario con ese nombre y contraseña.");
                ViewBag.Usuario = usuario;
                return View();
            }

        }

    }
}
