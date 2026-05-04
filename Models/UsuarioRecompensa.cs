using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient; 
using Dapper; 

namespace AlToque.Models;

public class UsuarioRecompensa
{
    public int idUsuario {get; set;}
    public int idRecompensa {get; set;}
    public DateTime fechaCompra {get; set;}
}
