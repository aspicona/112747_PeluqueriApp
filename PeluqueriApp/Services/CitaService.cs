using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class CitaService : ICitaService
    {
        private readonly AppDbContext _context;

        public CitaService(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las citas
        public async Task<IEnumerable<Cita>> GetAllCitasAsync()
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Empleado)
                .Include(c => c.Empresa)
                .Include(c => c.EstadoCita)
                .ToListAsync();
        }

        // Obtener una cita por su ID
        public async Task<Cita> GetCitaByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Empleado)
                .Include(c => c.Empresa)
                .Include(c => c.EstadoCita)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Cita>> GetCitasByEmpresaIdAsync(int empresaId)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Empleado)
                .Include(c => c.Empresa)
                .Include(c => c.EstadoCita)
                .Where(c => c.IdEmpresa == empresaId)
                .ToListAsync();
        }

        // Agregar una nueva cita
        public async Task AddCitaAsync(Cita cita)
        {
            if (cita == null)
            {
                throw new ArgumentNullException(nameof(cita));
            }
            // Guardar la cita
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
        }


        // Actualizar una cita existente
        public async Task UpdateCitaAsync(Cita cita)
        {
            _context.Citas.Update(cita);
            await _context.SaveChangesAsync();
        }

        // Eliminar una cita
        public async Task DeleteCitaAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                cita.Activo = false; // Marcar como inactiva
                _context.Citas.Update(cita); // Actualizar la cita en la base de datos
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<IngresoPorCita>> ObtenerIngresosPorCitasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Citas
                .Where(c => c.Fecha >= fechaInicio && c.Fecha <= fechaFin && c.IdEstadoCita == 4)
                .GroupBy(c => c.Fecha.Date)
                .Select(g => new IngresoPorCita
                {
                    Fecha = g.Key.ToString("dd/MM/yyyy"),
                    Ingreso = g.Sum(c => c.PrecioFinal)
                }).ToListAsync();
        }
        public async Task<List<string>> GetAvailableSlotsAsync(DateTime fecha, int empleadoId, int duracionTotal)
        {
            var slotsReservados = await _context.SlotsReservados
                .Where(s => s.Fecha.Date == fecha.Date && s.IdEmpleado == empleadoId)
                .Select(s => new { s.HoraInicio, s.HoraFin })
                .ToListAsync();

            var slotsDisponibles = new List<string>();
            var horaInicioDia = TimeSpan.FromHours(9);
            var horaFinDia = TimeSpan.FromHours(18);
            var duracionSlot = TimeSpan.FromMinutes(30);

                    for (var slot = horaInicioDia; slot < horaFinDia; slot += duracionSlot)
                    {
                        var horarioFin = slot + TimeSpan.FromMinutes(duracionTotal);

                        // Si el bloque excede el horario permitido, detén el bucle
                        if (horarioFin > horaFinDia)
                            break;

                        // Verificar si el bloque está disponible
                        if (!slotsReservados.Any(r =>
                            (slot >= r.HoraInicio && slot < r.HoraFin) ||             // Inicio dentro del rango reservado
                            (horarioFin > r.HoraInicio && horarioFin <= r.HoraFin) || // Fin dentro del rango reservado
                            (slot < r.HoraInicio && horarioFin > r.HoraFin)))         // El bloque abarca completamente un rango reservado
                        {
                            slotsDisponibles.Add(slot.ToString(@"hh\:mm"));
                        }
                    }
                    return slotsDisponibles;
        }

        public async Task ReservarSlotsAsync(DateTime fecha, int empleadoId, TimeSpan horarioInicio, int duracionMinutos, int citaId)
        {
            var horarioFin = horarioInicio + TimeSpan.FromMinutes(duracionMinutos);
            var slotsReservados = new List<SlotReservado>();

            for (var slot = horarioInicio; slot < horarioFin; slot += TimeSpan.FromMinutes(30))
            {
                // Reservar el slot completo
                slotsReservados.Add(new SlotReservado
                {
                    Fecha = fecha,
                    IdEmpleado = empleadoId,
                    HoraInicio = slot,
                    HoraFin = slot + TimeSpan.FromMinutes(30), // Fin del slot
                    IdCita = citaId
                });
            }

            // Si el tiempo total no coincide exactamente con los slots de 30 minutos, añadir el último slot
            if (horarioFin > slotsReservados.Last().HoraFin)
            {
                slotsReservados.Add(new SlotReservado
                {
                    Fecha = fecha,
                    IdEmpleado = empleadoId,
                    HoraInicio = slotsReservados.Last().HoraFin, // El inicio del siguiente slot
                    HoraFin = slotsReservados.Last().HoraFin + TimeSpan.FromMinutes(30),
                    IdCita = citaId
                });
            }

            await _context.SlotsReservados.AddRangeAsync(slotsReservados);
            await _context.SaveChangesAsync();
        }

        public async Task CancelarReservasAsync(int citaId)
        {
            var slotsReservados = await _context.SlotsReservados
                .Where(slot => slot.IdCita == citaId)
                .ToListAsync();

            if (slotsReservados.Any())
            {
                _context.SlotsReservados.RemoveRange(slotsReservados);
                await _context.SaveChangesAsync();
            }
        }
        public async Task EnviarMensajeWhatsAppAsync(string numeroCliente, CitaViewModel model)
        {
            // Token de acceso a la API de WhatsApp
            string token = "EAARomsxog1EBOzdlG7Ng0vgVtMEN4cejfA3tPRN0TWS6Eo6aWfqaUudUBdfHhZBiwkk3pjdypXsJXRw64ecFq1fXOBWWZCmV99zEOAtkjZAHiJtKguStvozV9KrgxOxfIcbUaopZCUeWeGBX53RmMEAMxyZBL5c6XFLpjZCjohHNeZA21Dk4BGOGA6XtXJ5YRZAe57Knx3VaZAgXPyhHSWnUR8XPtZBT4ZD";

            // Identificador del número de teléfono registrado en WhatsApp Business
            string idTelefono = "443491182189871";

            // URL para la solicitud
            string url = $"https://graph.facebook.com/v15.0/{idTelefono}/messages";

            Cliente cliente= await _context.Clientes
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(c => c.Id == model.IdCliente && c.Activo);

            List<string> nombresServicios = await _context.Servicios
                .Where(s => model.ServiciosSeleccionados.Contains(s.Id))
                .Select(s => s.Nombre)
                .ToListAsync();

            string listaServicios = string.Join(", ", nombresServicios);

            // Cuerpo del mensaje de WhatsApp
            var mensajeJson = $@"
            {{
                ""messaging_product"": ""whatsapp"",
                ""to"": ""{numeroCliente}"",
                ""type"": ""template"",
                ""template"": {{
                    ""name"": ""reserva_de_cita"",
                    ""language"": {{ ""code"": ""es"" }},
                    ""components"": [
                        {{
                            ""type"": ""body"",
                            ""parameters"": [
                                {{ ""type"": ""text"", ""text"": ""{cliente.Nombre}"" }},
                                {{ ""type"": ""text"", ""text"": ""{model.Fecha:dd/MM/yyyy}"" }},
                                {{ ""type"": ""text"", ""text"": ""{model.HorarioSeleccionado}"" }},
                                {{ ""type"": ""text"", ""text"": ""{listaServicios}"" }},
                                {{ ""type"": ""text"", ""text"": ""{cliente.Empresa.Direccion}"" }}
                            ]
                        }}
                    ]
                }}
            }}";

            using (HttpClient client = new HttpClient())
            {
                // Configurar la solicitud HTTP
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Content = new StringContent(mensajeJson);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Enviar la solicitud y manejar la respuesta
                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al enviar mensaje por WhatsApp: {response.StatusCode}, Detalle: {error}");
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Mensaje enviado correctamente: " + responseBody);
                }
            }
        }
        public async Task<List<ServicioRealizadoViewModel>> ObtenerServiciosRealizadosAsync(DateTime startDate, DateTime endDate, int empresaId)
        {
            return await _context.ServiciosXcita
                .Where(sxc =>
                    sxc.Cita.Fecha >= startDate &&
                    sxc.Cita.Fecha <= endDate &&
                    sxc.Cita.IdEmpresa == empresaId) // Filtrar por IdEmpresa
                .GroupBy(sxc => new { sxc.Servicio.Id, sxc.Servicio.Nombre }) // Agrupar por Id y Nombre del Servicio
                .Select(group => new ServicioRealizadoViewModel
                {
                    Id = group.Key.Id, // Usar el Id del grupo
                    NombreServicio = group.Key.Nombre,
                    Cantidad = group.Count()
                })
                .ToListAsync();
        }

    }
}
