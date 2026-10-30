Hito 1 - Sistema de Mesa de Ayuda
Objetivo
Desarrollar una primera versión funcional de un sistema de mesa de ayuda en C#.

Funcionalidades implementadas
Registro de tickets
Permite registrar tickets con:

Título
Descripción
Prioridad
Cada ticket recibe un identificador único y un estado inicial de Abierto.

Listado de tickets
Permite visualizar todos los tickets registrados mostrando:

ID
Título
Estado
Consulta de tickets
Permite consultar la información completa de un ticket mediante su ID.

Cambio de estado
Permite cambiar el estado de un ticket de Abierto a Cerrado.

El sistema evita cerrar un ticket que ya se encuentra cerrado.

Estadísticas
Muestra:

Total de tickets
Tickets abiertos
Tickets cerrados
Validaciones
El sistema valida:

Título obligatorio
Descripción obligatoria
Prioridad válida (Alta, Media o Baja)
Clases implementadas
Ticket
Propiedades:

Id
Titulo
Descripcion
Prioridad
Estado
Métodos:

Cerrar()
Conceptos aplicados
Métodos
Clases
Objetos
Encapsulamiento
Listas
LINQ
Validaciones
Cambios realizados respecto al Hito 0
Implementación de la clase Ticket.
Desarrollo del menú principal.
Registro de tickets.
Consulta por ID.
Cambio de estado.
Estadísticas generales.
Validaciones de entrada.
