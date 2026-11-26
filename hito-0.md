# Hito 0: Propuesta y modelo inicial

**Proyecto:** Mesa de ayuda del laboratorio de cómputo
**Integrante:** Oliver  González y Lidia Montezuma

---

## 1. Situación y usuario

Hoy, en el laboratorio de cómputo de una universidad, las solicitudes de soporte llegan por varios caminos a la vez: un mensaje de WhatsApp al encargado, un comentario verbal en el pasillo, un correo al coordinador o una nota en papel pegada en la puerta. Cada canal guarda la información en un lugar distinto y ninguno tiene un formato común. Por eso es habitual que una misma falla se reporte dos veces, que otra se pierda entre mensajes y que nadie recuerde quién la reportó ni cuándo. Además, quien reporta no recibe confirmación de que su solicitud fue recibida, así que suele insistir por otro canal y genera aún más duplicados.

El seguimiento se dificulta por cuatro motivos. Primero, no hay un registro único: para saber qué está pendiente, el encargado tiene que revisar chats, correos y notas. Segundo, las solicitudes llegan incompletas; con frases como «la computadora no sirve» no se sabe cuál equipo, en qué aula ni qué síntoma presenta. Tercero, no existe un estado claro: una solicitud puede estar pendiente, en revisión o resuelta, pero eso solo se sabe porque alguien lo recuerda. Cuarto, no hay forma de priorizar: un proyector dañado minutos antes de una clase pesa lo mismo que una silla floja en la lista mental de quien atiende.

El sistema propuesto es una mesa de ayuda sencilla para registrar y dar seguimiento a solicitudes de soporte técnico. Lo usaría principalmente el técnico de soporte del laboratorio, que necesita ver en una sola lista todas las solicitudes, filtrarlas por estado, abrir el detalle de cada una y actualizar su avance. De forma secundaria lo usaría el coordinador del laboratorio, que solo consulta cuántas solicitudes siguen abiertas y cuáles llevan más tiempo sin atención. Quien reporta el problema (un docente o un estudiante) sería registrado por el técnico a partir de lo que cuenta; en esta primera versión no habrá inicio de sesión ni cuentas.

Para trabajar bien, el técnico necesita saber de cada ticket qué ocurrió (título y descripción), de qué tipo es (categoría), qué tan urgente es (prioridad), quién lo reportó (un nombre de muestra), cuándo se registró y en qué estado está. También necesita que el sistema le avise cuando falte un dato o cuando intente un cambio de estado que no tiene sentido, por ejemplo, cerrar un ticket que nunca se empezó a atender. Con esa información, las decisiones diarias dejan de depender de la memoria de una sola persona.

Esta propuesta parte de una necesidad general que cualquier laboratorio conoce. No describe solicitudes reales ni menciona personas, y todos los datos de los ejemplos son inventados. El objetivo de la primera versión es demostrar que un modelo claro de ticket, con reglas simples, ya mejora el orden y la trazabilidad frente al método actual.

---

## 2. Acciones y resultados

| Acción | Cómo se hace | Qué muestra el sistema después |
|---|---|---|
| **Registrar** | El técnico pulsa **+ Nuevo ticket**, llena título, descripción, categoría, prioridad y solicitante, y pulsa **Guardar**. | Si todo es válido: mensaje verde «Ticket #4 creado», el formulario se limpia y el ticket aparece al inicio de la lista con estado **Abierto**. Si hay errores: mensajes en rojo junto a cada campo y el ticket **no** se guarda. |
| **Listar y encontrar** | La lista de la izquierda muestra todos los tickets. Se puede filtrar por estado y buscar por texto en el título. | Tabla con #, título, prioridad y estado. Si no hay coincidencias: «No se encontraron tickets con ese filtro». |
| **Consultar** | El técnico hace clic en una fila de la lista. | Panel de detalle a la derecha con todos los datos del ticket: título, descripción, categoría, prioridad, solicitante, fechas y estado actual. |
| **Cambiar de estado** | En el detalle, elige el nuevo estado en el selector y pulsa **Aplicar**. | Si el cambio es permitido: mensaje «Ticket #4 pasó a En progreso», se actualiza el estado y la fecha de actualización, tanto en el detalle como en la lista. Si no lo es: mensaje rojo explicando el motivo y el estado no cambia. |

Flujo de estados permitido:

```
Abierto ──► En progreso ──► Resuelto ──► Cerrado
                ▲              │
                └── (reabrir) ─┘
```

---

## 3. Clases iniciales

### Clase `Ticket`

**Datos**

| Dato | Tipo | Descripción |
|---|---|---|
| `Id` | int | Número único que asigna el sistema. |
| `Titulo` | string | Resumen corto del problema. |
| `Descripcion` | string | Detalle: equipo, aula, síntoma. |
| `Categoria` | enum | Hardware, Software, Red u Otro. |
| `Prioridad` | enum | Baja, Media o Alta. |
| `Solicitante` | string | Nombre de muestra de quien reportó. |
| `Estado` | enum | Abierto, EnProgreso, Resuelto o Cerrado. |
| `FechaCreacion` | DateTime | Cuándo se registró. |
| `FechaActualizacion` | DateTime | Último cambio de estado. |

**Regla propia de Ticket:** el método `CambiarEstado(nuevoEstado)` solo acepta las transiciones del flujo de la sección 2 (Abierto → En progreso → Resuelto → Cerrado, y Resuelto → En progreso para reabrir). Un ticket **Cerrado** no admite más cambios. Si la transición no es válida, el ticket conserva su estado y el método informa el motivo.

### Clase `RepositorioTickets`

Tiene una responsabilidad distinta: **guardar y buscar** tickets. Mantiene la lista de tickets en memoria (sin base de datos en esta etapa) y ofrece `Agregar(ticket)`, `ObtenerTodos()`, `Buscar(texto, estado)` y `ObtenerPorId(id)`. Además asigna el `Id` siguiente.

**Colaboración:** la interfaz (Blazor) crea un `Ticket` y se lo entrega al repositorio para guardarlo; para listar o consultar, pide los tickets al repositorio; para cambiar estado, obtiene el ticket del repositorio y llama a `ticket.CambiarEstado(...)`. El repositorio no conoce las reglas de estados y el `Ticket` no sabe cómo se almacena.

```
  Interfaz Blazor
     │        │
     │ crea / │ pide tickets
     │ llama  ▼
     │   RepositorioTickets ──── guarda ───► [ Ticket, Ticket, ... ]
     ▼
   Ticket.CambiarEstado()
```

---

## 4. Reglas

### Regla 1: Título obligatorio (5 a 80 caracteres)

| | Entrada | Respuesta del sistema |
|---|---|---|
| ✅ Válida | `Proyector del aula 3 no enciende` | Ticket guardado. «Ticket #4 creado». |
| ❌ Inválida | `PC` (2 caracteres) | No se guarda. Mensaje: «El título debe tener entre 5 y 80 caracteres». |
| ❌ Inválida | (vacío) | No se guarda. Mensaje: «El título es obligatorio». |

### Regla 2: Descripción obligatoria (10 a 500 caracteres)

| | Entrada | Respuesta del sistema |
|---|---|---|
| ✅ Válida | `El cable HDMI está suelto y la imagen se corta.` | Se acepta el campo. |
| ❌ Inválida | `No sirve` (8 caracteres) | No se guarda. Mensaje: «La descripción debe tener entre 10 y 500 caracteres». |

### Regla 3: Cambio de estado solo en el orden permitido

| | Entrada | Respuesta del sistema |
|---|---|---|
| ✅ Válida | Ticket **Abierto** → **En progreso** | Estado actualizado. «Ticket #4 pasó a En progreso». |
| ❌ Inválida | Ticket **Abierto** → **Cerrado** | El estado no cambia. Mensaje: «No se puede pasar de Abierto a Cerrado. Siguiente estado permitido: En progreso». |

### Regla 4: Un ticket cerrado no se modifica

| | Entrada | Respuesta del sistema |
|---|---|---|
| ✅ Válida | Ticket **Resuelto** → **Cerrado** | Estado actualizado. «Ticket #2 pasó a Cerrado». |
| ❌ Inválida | Ticket **Cerrado** → **En progreso** | El estado no cambia. Mensaje: «El ticket #2 está cerrado y ya no puede modificarse». |

### Regla 5: Categoría y prioridad deben elegirse de la lista

| | Entrada | Respuesta del sistema |
|---|---|---|
| ✅ Válida | Categoría `Red`, prioridad `Alta` | Se aceptan. |
| ❌ Inválida | Categoría sin seleccionar | No se guarda. Mensaje: «Seleccione una categoría». |

---

## 5. Boceto de interfaz

![Boceto de la interfaz de la mesa de ayuda](boceto-interfaz.png)
<img width="1400" height="900" alt="WhatsApp Image 2026-10-04 at 1 33 19 PM" src="https://github.com/user-attachments/assets/3c78b263-c27e-4001-be88-b46d27593e7a" />

Los números del boceto corresponden a las acciones de la sección 2: **1** Registrar, **2** Listar y encontrar, **3** Consultar, **4** Cambiar de estado. Los mensajes de validación aparecen en rojo junto a cada campo y los de confirmación en una barra verde al pie.

---

## 6. Pregunta y próxima acción

**Pregunta para el docente:** En Hito 1, ¿los tickets pueden vivir solo en memoria mientras la aplicación está abierta, o debemos prever desde ya que se conserven al cerrarla? Esto define si `RepositorioTickets` puede ser una simple lista.

**Primera acción concreta:** crear la solución en C# con la clase `Ticket` (enums `Estado`, `Categoria` y `Prioridad` y el método `CambiarEstado`) y escribir tres pruebas rápidas de las transiciones de la Regla 3, antes de construir cualquier pantalla de Blazor.

---

## 7. Organización del trabajo

Trabajo de forma individual. Distribución del tiempo previsto:

| Parte | Tiempo | Evidencia prevista |
|---|---|---|
| Modelo (clases `Ticket` y `RepositorioTickets`, reglas) | 40 % | Este documento y la clase `Ticket` con sus enums. |
| Interfaz (boceto y pantallas Blazor) | 30 % | `boceto-interfaz.png` y la lista y formulario básicos funcionando. |
| Pruebas (casos válidos e inválidos de cada regla) | 30 % | Tabla de la sección 4 convertida en pruebas que pasan. |
