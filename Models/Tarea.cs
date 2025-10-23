using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient; 
using Dapper; 
namespace AlToque.Controllers;

public class Tarea
{
    public int idTarea {get; set;}
    public string titulo {get; set;}
    public string descripcion {get; set;}
    public DateTime fechaInicio {get; set;}
    public DateTime fechaFin {get; set;}
    public bool esActivo {get; set;}
    public int IdUsuario {get; set;}
}
