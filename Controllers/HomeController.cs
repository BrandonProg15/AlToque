using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;

namespace AlToque.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // ==================== Métodos básicos ====================

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PedirUsuario()
        {
            return View("PedirUsuario");
        }

        public IActionResult Tareas(List<Tarea> tareas,string titulo, string descripcion, DateTime fechaInicio, DateTime fechaFin, bool esActivo)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));
            tareas = BD.ListarTareas(idUsuario);
            ViewBag.tareas = tareas;

             if (fechaInicio < new DateTime(1753, 1, 1))
            fechaInicio = DateTime.Now; 

        if (fechaFin < new DateTime(1753, 1, 1))
            fechaFin = DateTime.Now.AddDays(1);

        esActivo = true;

        ViewBag.titulo = titulo;
        ViewBag.descripcion = descripcion;
        ViewBag.fechaInicio = fechaInicio;
        ViewBag.fechaFin = fechaFin;
        ViewBag.esActivo = esActivo;
        ViewBag.IdUsuario = idUsuario;


        BD.CrearTarea(titulo, descripcion, fechaInicio, fechaFin, esActivo, idUsuario);
            return View("Tareas");
        }

        public IActionResult IniciarSesion()
        {
            return View("IniciarSesion");
        }

        public IActionResult EditarTarea()
        {
            return View();
        }

        public IActionResult EliminarTarea()
        {
            return View();
        }

        // ==================== Registro e inicio de sesión ====================

        
         [HttpPost]
     public IActionResult CrearTarea(string titulo, string descripcion, DateTime fechaInicio, DateTime fechaFin, bool esActivo)
    {
        
        if (fechaInicio < new DateTime(1753, 1, 1))
            fechaInicio = DateTime.Now; 

        if (fechaFin < new DateTime(1753, 1, 1))
            fechaFin = DateTime.Now.AddDays(1);

        esActivo = true;

        int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));

        ViewBag.titulo = titulo;
        ViewBag.descripcion = descripcion;
        ViewBag.fechaInicio = fechaInicio;
        ViewBag.fechaFin = fechaFin;
        ViewBag.esActivo = esActivo;
        ViewBag.IdUsuario = idUsuario;


        BD.CrearTarea(titulo, descripcion, fechaInicio, fechaFin, esActivo, idUsuario);

        return View("CrearTarea");  
    }
    [HttpPost]
        public IActionResult CrearUsuario(string mail, string contrasenia)
        {
            ViewBag.mail = mail;
            ViewBag.contrasenia = contrasenia;

            int usuarioOK = BD.CrearUsuarioBASE(mail, contrasenia);
            int idUsuario = BD.TraerUsuario(mail);
            HttpContext.Session.SetString("IdUsuario", idUsuario.ToString());

            if (usuarioOK != 0)
            {
                return View("Preferencias");
            }
            else
            {
                return View("ErrorUsuario");
            }
        }

        [HttpPost]
        public IActionResult IniciarSesionINICIAR(string nombre, string contrasenia)
        {
            ViewBag.nombre = nombre;
            ViewBag.contrasenia = contrasenia;

            int usuarioOK = BD.IniciarSesionBASE(nombre, contrasenia);

            if (usuarioOK == 1)
            {
                int idUsuario = BD.TraerUsuario(nombre);
                HttpContext.Session.SetString("IdUsuario", idUsuario.ToString());
                return View("CrearUsuario");
            }
            else
            {
                return View("ErrorUsuario");
            }
        }

        // ==================== Preferencias ====================

        [HttpPost]
        public IActionResult CrearPreferencia(string nombre, string usuario, string metodos, string anioEscolar, string hobbies, string objetivos)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));

            ViewBag.nombre = nombre;
            ViewBag.usuario = usuario;
            ViewBag.metodos = metodos;
            ViewBag.anioEscolar = anioEscolar;
            ViewBag.hobbies = hobbies;
            ViewBag.objetivos = objetivos;

            int usuarioOK = BD.CrearPreferenciaBASE(nombre, usuario, metodos, anioEscolar, hobbies, objetivos, idUsuario);

            if (usuarioOK == 1)
            {
                return View("home");
            }
            else
            {
                return View("ErrorUsuario");
            }
        }

        // ==================== Tareas ====================

        public IActionResult TareasRecientes()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));

            List<Tarea> tareas = BD.ListarTareas(idUsuario);

            // Filtrar solo las tareas que vencen en menos de 10 días
            DateTime hoy = DateTime.Now;
            tareas = tareas
                .Where(t => t.fechaFin >= hoy && t.fechaFin <= hoy.AddDays(10))
                .ToList();

            return View("home", tareas);
        }

        [HttpPost]
        public IActionResult EditarTarea(string tituloViejo, string titulo, string descripcion, DateTime fechaInicio, DateTime fechaFin, bool esActivo)
        {
            if (fechaInicio < new DateTime(1753, 1, 1))
                fechaInicio = DateTime.Now;

            if (fechaFin < new DateTime(1753, 1, 1))
                fechaFin = DateTime.Now.AddDays(1);

            esActivo = true;

            ViewBag.tituloViejo = tituloViejo;
            ViewBag.titulo = titulo;
            ViewBag.descripcion = descripcion;
            ViewBag.fechaInicio = fechaInicio;
            ViewBag.fechaFin = fechaFin;
            ViewBag.esActivo = esActivo;

            int tareaOK = BD.EditarTarea(tituloViejo, titulo, descripcion, fechaInicio, fechaFin);

            if (tareaOK == 1)
                return View("CrearUsuario");
            else
                return View("ErrorUsuario");
        }

        [HttpPost]
        public IActionResult EliminarTarea(string titulo)
        {
            ViewBag.titulo = titulo;

            int tareaOK = BD.EliminarTarea(titulo);

            if (tareaOK == 1)
                return View("CrearUsuario");
            else
                return View("ErrorUsuario");
        }
    }
}
