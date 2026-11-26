using MesaAyuda.Models;
using MySqlConnector;

namespace MesaAyuda.Services;

public class TicketService
{
    private readonly string _connectionString;

    public TicketService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public async Task<List<Categoria>> GetCategoriasAsync()
    {
        var lista = new List<Categoria>();
        using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("SELECT Id, Nombre FROM Categorias ORDER BY Nombre", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            lista.Add(new Categoria { Id = reader.GetInt32(0), Nombre = reader.GetString(1) });
        return lista;
    }

    public async Task<List<Ticket>> GetTicketsAsync(string? estado, int? categoriaId)
    {
        var lista = new List<Ticket>();
        using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();
        var sql = @"SELECT t.Id, t.Titulo, t.Descripcion, t.CategoriaId, c.Nombre, t.Estado, t.FechaCreacion
                    FROM Tickets t INNER JOIN Categorias c ON c.Id = t.CategoriaId WHERE 1=1";
        if (!string.IsNullOrEmpty(estado)) sql += " AND t.Estado = @estado";
        if (categoriaId.HasValue) sql += " AND t.CategoriaId = @cat";
        sql += " ORDER BY t.Id DESC";
        using var cmd = new MySqlCommand(sql, conn);
        if (!string.IsNullOrEmpty(estado)) cmd.Parameters.AddWithValue("@estado", estado);
        if (categoriaId.HasValue) cmd.Parameters.AddWithValue("@cat", categoriaId.Value);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(new Ticket
            {
                Id = reader.GetInt32(0),
                Titulo = reader.GetString(1),
                Descripcion = reader.GetString(2),
                CategoriaId = reader.GetInt32(3),
                CategoriaNombre = reader.GetString(4),
                Estado = reader.GetString(5),
                FechaCreacion = reader.GetDateTime(6)
            });
        }
        return lista;
    }

    public async Task<(int total, int abiertos, int cerrados)> GetCantidadesAsync()
    {
        using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("SELECT COUNT(*), SUM(CASE WHEN Estado='Abierto' THEN 1 ELSE 0 END), SUM(CASE WHEN Estado='Cerrado' THEN 1 ELSE 0 END) FROM Tickets", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();
        return (reader.GetInt32(0), reader.IsDBNull(1) ? 0 : reader.GetInt32(1), reader.IsDBNull(2) ? 0 : reader.GetInt32(2));
    }

    public async Task CrearAsync(Ticket t)
    {
        using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("INSERT INTO Tickets (Titulo, Descripcion, CategoriaId, Estado, FechaCreacion) VALUES (@t,@d,@c,@e,@f)", conn);
        cmd.Parameters.AddWithValue("@t", t.Titulo);
        cmd.Parameters.AddWithValue("@d", t.Descripcion);
        cmd.Parameters.AddWithValue("@c", t.CategoriaId);
        cmd.Parameters.AddWithValue("@e", t.Estado);
        cmd.Parameters.AddWithValue("@f", t.FechaCreacion);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(Ticket t)
    {
        using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("UPDATE Tickets SET Titulo=@t, Descripcion=@d, CategoriaId=@c WHERE Id=@id", conn);
        cmd.Parameters.AddWithValue("@t", t.Titulo);
        cmd.Parameters.AddWithValue("@d", t.Descripcion);
        cmd.Parameters.AddWithValue("@c", t.CategoriaId);
        cmd.Parameters.AddWithValue("@id", t.Id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CerrarAsync(Ticket t)
    {
        t.Cerrar();
        using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("UPDATE Tickets SET Estado='Cerrado' WHERE Id=@id", conn);
        cmd.Parameters.AddWithValue("@id", t.Id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("DELETE FROM Tickets WHERE Id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await cmd.ExecuteNonQueryAsync();
    }
}