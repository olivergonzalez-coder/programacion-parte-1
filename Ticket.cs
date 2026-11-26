public class Ticket
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Prioridad { get; set; } = string.Empty;

    public string Estado { get; set; } = "Abierto";

    public string Categoria { get; set; } = string.Empty;

    public Ticket()
    {
    }

    public bool Cerrar()
    {
        if (Estado == "Cerrado")
            return false;

        Estado = "Cerrado";
        return true;
    }
}