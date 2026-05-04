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

        public class CalendarioViewModel
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int DaysInMonth { get; set; }
            public int StartWeekDay { get; set; }
        }

        private CalendarioViewModel GenerarCalendario(int? year, int? month)
        {
            int y = year ?? DateTime.Now.Year;
            int m = month ?? DateTime.Now.Month;

            if (m < 1)
            {
                m = 12;
                y--;
            }
            else if (m > 12)
            {
                m = 1;
                y++;
            }

            return new CalendarioViewModel
            {
                Year = y,
                Month = m,
                DaysInMonth = DateTime.DaysInMonth(y, m),
                StartWeekDay = ((int)new DateTime(y, m, 1).DayOfWeek + 6) % 7 + 1
            };
        }

        public IActionResult Calendario(int? year, int? month)
        {
            var model = GenerarCalendario(year, month);
            return View("home", model);
        }

        public IActionResult Home(int? year, int? month)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));
            int tukimonedas = BD.TraerTukimonedas(idUsuario);
            ViewBag.tukimonedas = tukimonedas;
            var model = GenerarCalendario(year, month);
            var tareas = BD.ListarTareas(idUsuario);
            ViewBag.tareas = tareas;
            return View("home", model);
        }

        public IActionResult Index() => View();
        public IActionResult PedirUsuario() => View("PedirUsuario");
        public IActionResult IniciarSesion() => View("IniciarSesion");
        public IActionResult EliminarTarea() => View();

        public IActionResult Metodos()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));
            int tukimonedas = BD.TraerTukimonedas(idUsuario);
            ViewBag.tukimonedas = tukimonedas;
            return View("Metodos");
        }

      public IActionResult Usuario()
{
    int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));

    var preferencias = BD.TraerPreferencias(idUsuario);


    if (preferencias != null)
    {
        ViewBag.usuario = preferencias.Usuario; 
        ViewBag.metodos = preferencias.MetodosUsas;
        ViewBag.anioEscolar = preferencias.AnioEscolar;
        ViewBag.hobbies = preferencias.Hobbies;
        ViewBag.objetivos = preferencias.Objetivos;
    }
    else
    {
        ViewBag.metodos = "";
        ViewBag.anioEscolar = "";
        ViewBag.hobbies = "";
        ViewBag.objetivos = "";
        ViewBag.usuario = "";
    }

    return View("Usuario");
}


        public IActionResult Recompensas()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));
            int tukimonedas = BD.TraerTukimonedas(idUsuario);
            List<Recompensa> recompensas = BD.TraerRecompensas();
            List<int> recompensasCompradas = BD.ObtenerRecompensasUsuario(idUsuario);
            var modelo = new Tuple<List<Recompensa>, List<int>>(recompensas, recompensasCompradas);
            ViewBag.tukimonedas = tukimonedas;

            return View(modelo);
        }

        [HttpPost]
        public IActionResult RestarTuks(int idRecompensa)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));
            int resultado = BD.RestarTukis(idUsuario, idRecompensa);
            int tukimonedas = BD.TraerTukimonedas(idUsuario);

            if (resultado == -1)
                return Json(new { success = false, message = "Ya compraste esta recompensa.", tukimonedas });

            if (resultado == -2)
                return Json(new { success = false, message = "No tenés suficientes tukimonedas.", tukimonedas });

            return Json(new { success = true, message = "Compra exitosa.", tukimonedas });
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
                return View("Preferencias");
            else
                return View("ErrorUsuario");
        }

        [HttpPost]
        public IActionResult IniciarSesionINICIAR(string mail, string contrasenia)
        {
            ViewBag.mail = mail;
            ViewBag.contrasenia = contrasenia;

            int usuarioOK = BD.IniciarSesionBASE(mail, contrasenia);

            if (usuarioOK > 1)
            {
                int idUsuario = BD.TraerUsuario(mail);
                HttpContext.Session.SetString("IdUsuario", idUsuario.ToString());
                idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));
                var tareas = BD.ListarTareas(idUsuario);
                ViewBag.tareas = tareas;
                return View("home", GenerarCalendario(null, null));
            }
            else
            {
                return View("ErrorUsuario");
            }
        }

        [HttpPost]
        public IActionResult CrearPreferencia(string nombre, string usuario, string metodos, string anioEscolar, string hobbies, string objetivos)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));

            HttpContext.Session.SetString("Nombre", nombre);
            HttpContext.Session.SetString("metodos", metodos);
            HttpContext.Session.SetString("anioEscolar", anioEscolar);
            HttpContext.Session.SetString("hobbies", hobbies);
            HttpContext.Session.SetString("objetivos", objetivos);

            ViewBag.nombre = nombre;
            ViewBag.usuario = usuario;
            ViewBag.metodos = metodos;
            ViewBag.anioEscolar = anioEscolar;
            ViewBag.hobbies = hobbies;
            ViewBag.objetivos = objetivos;

            int usuarioOK = BD.CrearPreferenciaBASE(nombre, usuario, metodos, anioEscolar, hobbies, objetivos, idUsuario);

            if (usuarioOK == 1)
                return View("home", GenerarCalendario(null, null));
            else
                return View("ErrorUsuario");
        }

        public IActionResult CancelarPreferencias()
        {
            return View("home", GenerarCalendario(null, null));
        }

        [HttpGet]
        public IActionResult Tareas()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));
            int tukimonedas = BD.TraerTukimonedas(idUsuario);
            var tareas = BD.ListarTareas(idUsuario);

            ViewBag.tareas = tareas;
            ViewBag.tukimonedas = tukimonedas;

            return View("Tareas");
        }

        [HttpPost]
        public IActionResult Tareas(string titulo, string descripcion, DateTime fechaInicio, DateTime fechaFin, bool esActivo)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));

            if (fechaInicio < new DateTime(1753, 1, 1)) fechaInicio = DateTime.Now;
            if (fechaFin < new DateTime(1753, 1, 1)) fechaFin = DateTime.Now.AddDays(1);

            esActivo = true;

            BD.CrearTarea(titulo, descripcion, fechaInicio, fechaFin, esActivo, idUsuario);

            return RedirectToAction("Tareas");
        }

        public IActionResult TareasRecientes()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IdUsuario"));
            List<Tarea> tareas = BD.ListarTareas(idUsuario);

            DateTime hoy = DateTime.Now;
            tareas = tareas.Where(t => t.fechaFin >= hoy && t.fechaFin <= hoy.AddDays(10)).ToList();

            return View("home", GenerarCalendario(null, null));
        }

        [HttpPost]
        public IActionResult EditarTarea(string tituloViejo, string titulo, string descripcion, DateTime fechaInicio, DateTime fechaFin, bool esActivo)
        {
            if (fechaInicio < new DateTime(1753, 1, 1)) fechaInicio = DateTime.Now;
            if (fechaFin < new DateTime(1753, 1, 1)) fechaFin = DateTime.Now.AddDays(1);

            esActivo = true;

            int tareaOK = BD.EditarTarea(tituloViejo, titulo, descripcion, fechaInicio, fechaFin);

                return View("Tareas");
        }

        [HttpPost]
        public IActionResult EliminarTarea(string titulo)
        {
            int tareaOK = BD.EliminarTarea(titulo);

            if (tareaOK == 1)
                return RedirectToAction("Tareas");
            else
                return View("ErrorUsuario");
        }
    }
}
