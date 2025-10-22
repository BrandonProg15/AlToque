using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;

namespace AlToque.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    int IdUsuario = 0; 
    public IActionResult Index()
    {
         return View();
    }
    public IActionResult CrearUsuario(string nombre, string contrasenia)
    {
        ViewBag.nombre = nombre;
        ViewBag.contrasenia = contrasenia;
        int usuarioOK = BD.CrearUsuarioBASE(nombre, contrasenia);
        if (usuarioOK == 1)
        {
        return View("CrearUsuario");
        }
        else{
            return View("ErrorUsuario");
        } 
    }
     public IActionResult IniciarSesionINICIAR(string nombre, string contrasenia)
    {
        ViewBag.nombre = nombre;
        ViewBag.contrasenia = contrasenia;
        int usuarioOK = BD.IniciarSesionBASE(nombre, contrasenia);
        IdUsuario = BD.TraerUsuario(nombre);
        HttpContext.Session.SetString("IdUsuario", IdUsuario.ToString());
        if (usuarioOK == 1)
        {
        return View("CrearUsuario"); 
        } 
        else{
            return View("ErrorUsuario");
        } 
    }
    public IActionResult PedirUsuario(string nombre, string contrasenia)
    {
        return View("PedirUsuario");
    }
    public IActionResult IniciarSesion(string nombre, string contrasenia)
    {
        return View("IniciarSesion");
    }
       public IActionResult ListarTareas(List<Tarea> tareas)
    {
        tareas = BD.ListarTareas();
        ViewBag.tareas = tareas;
        return View ("ListarTareas");
    }
     public IActionResult CrearTarea() {
        return View ();
     }

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

        return View("CrearUsuario");  
    }
      public IActionResult EditarTarea() {
        return View ();
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
        {
        return View("CrearUsuario");
        }
        else{
            return View("ErrorUsuario");
        } 
    }
    public IActionResult EliminarTarea() {
        return View ();
     }
        [HttpPost]
     public IActionResult EliminarTarea(string titulo)
    {
        ViewBag.titulo = titulo;

        int tareaOK = BD.EliminarTarea(titulo);

        if (tareaOK == 1)
        {
        return View("CrearUsuario");
        }
        else{
            return View("ErrorUsuario");
        } 
    }
}

