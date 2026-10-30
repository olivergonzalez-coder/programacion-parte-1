public class Ticket
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Prioridad { get; set; } = string.Empty;

    public string Estado { get; private set; } = "Abierto";

    public Ticket(int id,
                  string titulo,
                  string descripcion,
                  string prioridad)
    {
        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
        Prioridad = prioridad;

        Estado = "Abierto";
    }

    public bool Cerrar()
    {
        if (Estado == "Cerrado")
            return false;

        Estado = "Cerrado";

        return true;
    }
}