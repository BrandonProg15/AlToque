using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient; 
using Dapper; 

namespace AlToque.Models;

public class Recompensa
{
    public int idRecompensa {get; set;}
    public string nombre {get; set;}
    public int costo {get; set;}
}