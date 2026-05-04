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
    
    public static Preferencias TraerPreferencias(int idUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string storedProcedure = "traerPreferencias";

        var preferencias = connection.Query<Preferencias>(
            storedProcedure,
            new { idUsuario = idUsuario },
            commandType: CommandType.StoredProcedure
        ).FirstOrDefault();

        return preferencias;
    }
}

public static int TraerTukimonedas(int idUsuario)
{
    int tukimonedas = 0;
    string sql = "SELECT tukimonedas FROM Usuario WHERE idUsuario = @idUsuario";

    using (SqlConnection conexion = new SqlConnection(_connectionString))
    {
        tukimonedas = conexion.QueryFirstOrDefault<int>(sql, new { idUsuario });
    }

    return tukimonedas;
}
public static List<int> ObtenerRecompensasUsuario(int idUsuario)
{
    List<int> listaRecompensas = new List<int>();
    
    using (SqlConnection con = new SqlConnection(_connectionString))
    {
        string sql = "SELECT idRecompensa FROM UsuarioRecompensa WHERE idUsuario = @idUsuario";
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        
        con.Open();
        
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            listaRecompensas.Add(reader.GetInt32(0));
        }
        
        reader.Close();
    }
    
    return listaRecompensas;
}
  public static List<Recompensa> TraerRecompensas()
    {
        List<Recompensa> recompensas = new List<Recompensa>();

        using (SqlConnection connection = new SqlConnection(_connectionString)) 
        {
            string query = "SELECT idRecompensa, nombre, costo FROM Recompensa";
            recompensas = connection.Query<Recompensa>(query).ToList();
        }
        return recompensas;
    }
public static int RestarTukis(int idUsuario, int idRecompensa)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        var parameters = new DynamicParameters();
        parameters.Add("@idUsuario", idUsuario);
        parameters.Add("@idRecompensa", idRecompensa);
        parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

        connection.Execute("restarTukimonedas", parameters, commandType: CommandType.StoredProcedure);

        return parameters.Get<int>("ReturnValue");
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

        var parametros = new
        {
            nombre = nombre,
            usuario = usuario,
            metodo = metodos,
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
    public static int IniciarSesionBASE(string mail, string contrasenia)
    {
        int usuarioOk;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "IniciarSesion";

            usuarioOk = connection.QueryFirstOrDefault<int>(
            storedProcedure,
            new { email = mail, contrasenia = contrasenia },
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








