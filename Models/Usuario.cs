using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient; 
using Dapper; 

namespace AlToque.Controllers;

public class Usuario
{
    public int idUsuario {get; set;}
    public string nombre {get; set;}
    public string contrasenia {get; set;}

}
