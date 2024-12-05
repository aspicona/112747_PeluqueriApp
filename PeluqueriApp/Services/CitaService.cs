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
                _context.Citas.Remove(cita);
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
        public async Task<List<string>> GetAvailableSlotsAsync(DateTime fecha, int empleadoId, int duracionMinutos)
        {
            // Obtener los slots ya reservados para este empleado en la fecha especificada
            var slotsReservados = await _context.SlotsReservados
                .Where(s => s.Fecha.Date == fecha.Date && s.IdEmpleado == empleadoId)
                .Select(s => new { s.HoraInicio, s.HoraFin })
                .ToListAsync();

            // Crear una lista de todos los slots posibles (30 minutos de duración) en el día
            var slotsDisponibles = new List<string>();
            var horaInicioDia = TimeSpan.FromHours(9);  // Asumimos que el día comienza a las 9:00 a.m.
            var horaFinDia = TimeSpan.FromHours(18);   // Asumimos que el día termina a las 6:00 p.m.

            for (var slot = horaInicioDia; slot < horaFinDia; slot += TimeSpan.FromMinutes(30))
            {
                var horarioFinSlot = slot + TimeSpan.FromMinutes(duracionMinutos);

                // Verificar si el slot no está reservado
                if (!slotsReservados.Any(r =>
                    (slot >= r.HoraInicio && slot < r.HoraFin) ||
                    (horarioFinSlot > r.HoraInicio && horarioFinSlot <= r.HoraFin)))
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
                slotsReservados.Add(new SlotReservado
                {
                    Fecha = fecha,
                    IdEmpleado = empleadoId,
                    HoraInicio = slot,
                    HoraFin = slot + TimeSpan.FromMinutes(30),
                    IdCita = citaId // Relacionado siempre con una cita
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
        public async Task EnviarMensajeWhatsAppAsync(string numeroCliente)
        {
            // Token de acceso a la API de WhatsApp
            string token = "EAARomsxog1EBO9msJlbUUKdSdPQnZCDt7w0X0BrOIsKeUBt2mnANXXjDAeptZAmjVU0WZC77ymebX3Lw8RAWiEoUJjti1uThmmmrwZBLiSWBCxfgYPHGENhTDJz3U6ZBWPRSKlpI0GhYalfPVdAubFbohpZAZAPmdIgVKuSmmX8oLctMJHzPI1tIJJYiIt4VcrkvMnTmlXNsFBNBlc2yLu6WlkO4GMZD";

            // Identificador del número de teléfono registrado en WhatsApp Business
            string idTelefono = "443491182189871";

            // URL para la solicitud
            string url = $"https://graph.facebook.com/v15.0/{idTelefono}/messages";

            // Cuerpo del mensaje de WhatsApp
            string mensajeJson = $@"
            {{
                ""messaging_product"": ""whatsapp"",
                ""to"": ""{numeroCliente}"",
                ""type"": ""template"",
                ""template"": {{
                    ""name"": ""hello_world"",
                    ""language"": {{ ""code"": ""en_US"" }}
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
