using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient; 
using Dapper; 
namespace AlToque.Controllers;

public class UsuarioxTarea
{
    public int IdUsuarioxTarea {get; set;}
    public int IdUsuario {get; set;}
    public int IdTarea {get; set;}
}
