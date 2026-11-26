Hito 2 - Mesa de Ayuda con Persistencia en MySQL
Objetivo
Pasar del Hito 1 (datos en memoria) al Hito 2 (datos guardados en MySQL). La app ya no pierde los tickets al cerrarla.

Qué se hizo

Se creó la base de datos MesaDeAyudaDB en MySQL
Se creó la tabla Categorias con 4 categorías iniciales
Se creó la tabla Tickets con título, descripción, categoría, estado y fecha
Se conectó Blazor con MySQL usando MySqlConnector y ADO.NETSe mantuvo toda la interfaz del Hito 1 y se le agregó persistencia

Archivos importantes
Components/Pages/Home.razor - Pantalla principal, crear, editar, filtrar, ver detalle, cerrar y eliminar
Models/Ticket.cs - Modelo Ticket y Categoria, validación y regla de no cerrar 2 veces
Services/TicketService.cs - Todo el acceso a MySQL con consultas parametrizadas
crear_base_datos.sql - Script que crea la base y las tablas
appsettings.json - Cadena de conexión (sin contraseña real en el repo)

Cómo se probó
Se creó un ticket y se cerró Visual Studio, al volver a abrir el ticket seguía ahí
Se intentó crear un ticket con título vacío y lo rechazó
Se intentó cerrar 2 veces el mismo ticket y lo rechazó
Se filtró por Abierto / Cerrado y por categoría y no se borró ningún dato
Se probó con filtro sin resultados y salió el mensaje No hay coincidencias

Validación, errores y seguridad
El modelo rechaza título o descripción vacía o solo con espacios, y categoría sin elegir
El modelo rechaza cerrar un ticket ya cerrado
La interfaz muestra el error en rojo y no muestra éxito cuando algo falla
Si falla la conexión se muestra Error al conectar con la base y la app no se cae
Todas las consultas usan parámetros (@t, @d, @c, @id, etc.)
Nunca se concatena lo que escribe el usuario dentro del SQL, eso evita inyección SQL

Persistencia
Los tickets se guardan en MySQL. Al cerrar y volver a abrir la aplicación, los tickets siguen ahí.