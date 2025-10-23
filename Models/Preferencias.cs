using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient; 
using Dapper; 

namespace AlToque.Controllers;

public class Preferencias
{
    public int idPreferencia {get; set;}
    public string nombre {get; set;}
    public string metodo {get; set;}
    public string anioEscolar {get; set;}
    public string hobbies {get; set;}
    public string objetivos {get; set;}
    public string idUsuario {get; set;}


}
