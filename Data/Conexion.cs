using Npgsql;
using System.Threading.Tasks;
using System;

namespace library.Data
{
    public class Conexion
    {
        public string ConnectionString { get; } =
            "Host=ep-soft-truth-adn780tq-pooler.c-2.us-east-1.aws.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_8LsPxfKyH5du; SSL Mode=VerifyFull; Channel Binding=Require;";

        public NpgsqlConnection CrearConexion()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        // Método adicional para probar la conexión de manera asíncrona
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = CrearConexion();
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        
        
        public async Task<int> ExecuteNonQueryAsync(string query, NpgsqlParameter[]? parameters = null)
        {
            using var connection = CrearConexion();
            await connection.OpenAsync();
            
            using var command = new NpgsqlCommand(query, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            
            return await command.ExecuteNonQueryAsync();
        }

    }
}