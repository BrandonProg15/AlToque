using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient; 
using Dapper; 

namespace AlToque.Models;

public class Usuario
{
    public int idUsuario {get; set;}
    public string mail {get; set;}
    public string contrasenia {get; set;}  
    public string tukimonedas {get; set;}

}
