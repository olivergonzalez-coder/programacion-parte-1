using System;
using System.Collections.Generic;
using System.Linq;

List<Ticket> tickets = new();

int contadorId = 1;

int opcion;

do
{
    SafeClear();

    Console.WriteLine("===== MESA DE AYUDA =====");
    Console.WriteLine("1. Registrar ticket");
    Console.WriteLine("2. Listar tickets");
    Console.WriteLine("3. Ver ticket");
    Console.WriteLine("4. Cerrar ticket");
    Console.WriteLine("5. Estadisticas");
    Console.WriteLine("0. Salir");

    opcion = Convert.ToInt32(Console.ReadLine());

    switch (opcion)
    {
        case 1:
            RegistrarTicket();
            break;

        case 2:
            ListarTickets();
            break;

        case 3:
            VerTicket();
            break;

        case 4:
            CerrarTicket();
            break;

        case 5:
            MostrarEstadisticas();
            break;
    }
} while (opcion != 0);

void RegistrarTicket()
{
    Console.Write("Titulo: ");
    string titulo = Console.ReadLine() ?? string.Empty;

    Console.Write("Descripcion: ");
    string descripcion = Console.ReadLine() ?? string.Empty;

    Console.Write("Prioridad (Alta/Media/Baja): ");
    string prioridad = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(titulo))
    {
        Console.WriteLine("Titulo vacio");
        SafeReadKey();
        return;
    }

    if (string.IsNullOrWhiteSpace(descripcion))
    {
        Console.WriteLine("Descripcion vacia");
        SafeReadKey();
        return;
    }

    if (prioridad != "Alta" &&
        prioridad != "Media" &&
        prioridad != "Baja")
    {
        Console.WriteLine("Prioridad invalida");
        SafeReadKey();
        return;
    }

    Ticket nuevo = new Ticket(contadorId++, titulo, descripcion, prioridad);
    tickets.Add(nuevo);

    Console.WriteLine("Ticket registrado");
    SafeReadKey();
}

void ListarTickets()
{
    SafeClear();

    foreach (Ticket ticket in tickets)
    {
        Console.WriteLine($"{ticket.Id} | {ticket.Titulo} | {ticket.Estado}");
    }

    SafeReadKey();
}

void VerTicket()
{
    Console.Write("ID: ");

    int id = Convert.ToInt32(Console.ReadLine());

    Ticket ticket = tickets.FirstOrDefault(t => t.Id == id);

    if (ticket == null)
    {
        Console.WriteLine("No encontrado");
    }
    else
    {
        Console.WriteLine($"ID: {ticket.Id}");
        Console.WriteLine($"Titulo: {ticket.Titulo}");
        Console.WriteLine($"Descripcion: {ticket.Descripcion}");
        Console.WriteLine($"Prioridad: {ticket.Prioridad}");
        Console.WriteLine($"Estado: {ticket.Estado}");
    }

    SafeReadKey();
}

void CerrarTicket()
{
    Console.Write("ID: ");

    int id = Convert.ToInt32(Console.ReadLine());

    Ticket ticket = tickets.FirstOrDefault(t => t.Id == id);

    if (ticket == null)
    {
        Console.WriteLine("No encontrado");
    }
    else
    {
        if (ticket.Cerrar())
        {
            Console.WriteLine("Ticket cerrado");
        }
        else
        {
            Console.WriteLine("Ya estaba cerrado");
        }
    }

    SafeReadKey();
}

void MostrarEstadisticas()
{
    Console.WriteLine($"Total: {tickets.Count}");
    Console.WriteLine($"Abiertos: {tickets.Count(t => t.Estado == "Abierto")}");
    Console.WriteLine($"Cerrados: {tickets.Count(t => t.Estado == "Cerrado")}");

    SafeReadKey();
}

static void SafeClear()
{
    try
    {
        Console.Clear();
    }
    catch (IOException)
    {
    }
}

static void SafeReadKey()
{
    try
    {
        Console.ReadKey();
    }
    catch (InvalidOperationException)
    {
        Console.ReadLine();
    }
    catch (IOException)
    {
        Console.ReadLine();
    }
}
