namespace MesaAyuda.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class Ticket
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public string Estado { get; set; } = "Abierto";
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Para que lo pueda leer la base de datos
    public Ticket() { }

    public Ticket(string titulo, string descripcion, int categoriaId)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new Exception("El título es obligatorio.");
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new Exception("La descripción es obligatoria.");
        if (categoriaId <= 0)
            throw new Exception("Debes elegir una categoría.");

        Titulo = titulo.Trim();
        Descripcion = descripcion.Trim();
        CategoriaId = categoriaId;
        Estado = "Abierto";
        FechaCreacion = DateTime.Now;
    }

    public void Actualizar(string titulo, string descripcion, int categoriaId)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new Exception("El título es obligatorio.");
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new Exception("La descripción es obligatoria.");
        if (categoriaId <= 0)
            throw new Exception("Debes elegir una categoría.");

        Titulo = titulo.Trim();
        Descripcion = descripcion.Trim();
        CategoriaId = categoriaId;
    }

    public void Cerrar()
    {
        if (Estado == "Cerrado")
            throw new Exception("El ticket ya está cerrado.");
        Estado = "Cerrado";
    }
}