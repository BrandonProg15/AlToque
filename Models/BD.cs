using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlToque.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace AlToque.Controllers;

public static class BD
{
    private static string _connectionString = @"Server=localhost; DataBase=AlToque;Integrated Security=True;TrustServerCertificate=True;";
    private static int IdUsuario = 0;

    public static List<Tarea> ListarTareas(int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "ListarTareas";
            var tareas = connection.Query<Tarea>(
                storedProcedure,
                 new { idUsuario = idUsuario},
                commandType: CommandType.StoredProcedure
            ).ToList();
            return tareas;
        }
    } 
    public static void CrearTarea(string titulo, string descripcion, DateTime fechaInicio, DateTime fechaFin, bool esActivo, int IdUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "CrearTarea";

            connection.QueryFirstOrDefault<string>(
            storedProcedure,
            new { titulo = titulo, descripcion = descripcion, fechaInicio = fechaInicio, fechaFin = fechaFin, esActivo = esActivo, IdUsuario = IdUsuario },
            commandType: CommandType.StoredProcedure);
        }
    }
    public static int EditarTarea(string tituloViejo, string titulo, string descripcion, DateTime fechaInicio, DateTime fechaFin)
    {
        int tareaOk;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "EditarTarea";

            tareaOk = connection.QueryFirstOrDefault<int>(
            storedProcedure,
            new { tituloViejo = tituloViejo, titulo = titulo, descripcion = descripcion, fechaInicio = fechaInicio, fechaFin = fechaFin },
            commandType: CommandType.StoredProcedure);
            return tareaOk;
        }
    }
    public static int CrearUsuarioBASE(string mail, string contrasenia)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string storedProcedure = "CrearUsuario";

      int idUsuario = connection.QuerySingleOrDefault<int>(
    storedProcedure,
    new { mail, contrasenia },
    commandType: CommandType.StoredProcedure
    );
    return idUsuario;

    }
}

    public static int CrearPreferenciaBASE(
    string nombre,
    string usuario,
    string metodos,
    string anioEscolar,
    string hobbies,
    string objetivos,
    int idUsuario
)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string storedProcedure = "CrearPreferencia";

        // Aquí los nombres del objeto deben coincidir con los parámetros del SP
        var parametros = new
        {
            nombre = nombre,
            usuario = usuario,      // coincide con @usuario en el SP
            metodo = metodos,       // coincide con @metodo en el SP
            anioEscolar = anioEscolar,
            hobbies = hobbies,
            objetivos = objetivos,
            idUsuario = idUsuario
        };

        return connection.Execute(
            storedProcedure,
            parametros,
            commandType: CommandType.StoredProcedure
        );
    }
}
    public static int IniciarSesionBASE(string nombre, string contrasenia)
    {
        int usuarioOk;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "IniciarSesion";

            usuarioOk = connection.QueryFirstOrDefault<int>(
            storedProcedure,
            new { nombre = nombre, contrasenia = contrasenia },
            commandType: CommandType.StoredProcedure);
            return usuarioOk;
        }
    }
    public static int EliminarTarea(string titulo)
    {
        int tareaOk;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "EliminarTarea";

            tareaOk = connection.Execute(
            storedProcedure,
            new { titulo = titulo },
            commandType: CommandType.StoredProcedure);
            return tareaOk;
        }
    }
    public static int TraerUsuario(string mail)
    {
        int IdUsuario;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "TraerUsuario";

            IdUsuario = connection.QueryFirstOrDefault<int>(
            storedProcedure,
            new { mail = mail },
            commandType: CommandType.StoredProcedure);
        }
        return IdUsuario;
    }

}






