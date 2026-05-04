using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient; 
using Dapper; 

namespace AlToque.Models;

public class Preferencias
{
    public int idPreferencia {get; set;}
    public string nombre {get; set;}
    public string MetodosUsas {get; set;}
    public string AnioEscolar {get; set;}
    public string Hobbies {get; set;}
    public string Objetivos {get; set;}
    public int idUsuario {get; set;}
    public string Usuario {get; set;}
}
