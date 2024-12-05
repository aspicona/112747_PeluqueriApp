using Microsoft.AspNetCore.Mvc;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

public class CitaController : Controller
{
    private readonly ICitaService _citaService;
    private readonly IClienteService _clienteService;
    private readonly IEmpleadoService _empleadoService;
    private readonly IEmpresaService _empresaService;
    private readonly IEstadoCitaService _estadoCitaService;
    private readonly IServicioService _servicioService;
    private readonly IProductoService _productoService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CitaController(ICitaService citaService, IClienteService clienteService,
        IEmpleadoService empleadoService, IEmpresaService empresaService,
        IEstadoCitaService estadoCitaService, UserManager<ApplicationUser> userManager, IServicioService servicioService,
        IProductoService productoService)
    {
        _citaService = citaService;
        _clienteService = clienteService;
        _empleadoService = empleadoService;
        _empresaService = empresaService;
        _estadoCitaService = estadoCitaService;
        _userManager = userManager;
        _servicioService = servicioService;
        _productoService = productoService;
    }

    private async Task<int?> GetEmpresaIdFromUser()
    {
        var user = await _userManager.GetUserAsync(User);
        return user?.IdEmpresa;
    }
    private async Task SetViewBagsAsync()
    {
        var empresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault();
        if (empresaId == 0)
        {
            throw new UnauthorizedAccessException("No se encontró una empresa asociada al usuario actual.");
        }

        // Clientes
        var clientes = await _clienteService.GetClientesByEmpresaIdAsync(empresaId);
        ViewBag.Clientes = new SelectList(clientes.Select(c => new
        {
            Id = c.Id,
            NombreCompleto = $"{c.Nombre} {c.Apellido}"
        }), "Id", "NombreCompleto");

        // Empleados
        var empleados = await _empleadoService.GetEmpleadosByEmpresaIdAsync(empresaId);
        ViewBag.Empleados = new SelectList(empleados.Select(e => new
        {
            Id = e.Id,
            NombreCompleto = $"{e.Nombre} {e.Apellido}"
        }), "Id", "NombreCompleto");

        // Servicios
        var servicios = await _servicioService.GetServiciosByEmpresaIdAsync(empresaId);
        ViewBag.Servicios = new SelectList(servicios, "Id", "Nombre");

        // Productos
        var productos = await _productoService.GetProductosByEmpresaIdAsync(empresaId);
        ViewBag.Productos = productos.Select(p => new ProductoViewModel
        {
            ProductoId = p.ProductoId,
            NombreProducto = p.NombreProducto,
            Cantidad = 0,
            Seleccionado = false
        }).ToList();

        // Estados de cita
        var estadosCita = await _estadoCitaService.GetAllEstadosAsync();
        ViewBag.EstadosCita = new SelectList(estadosCita, "Id", "Descripcion");
    }

    public async Task<IActionResult> Index()
    {
        // Obtén el Id de la empresa asociada al usuario actual
        var empresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault();

        if (empresaId == 0)
        {
            return Unauthorized(); 
        }

        // Traer las citas de la empresa
        var citas = await _citaService.GetCitasByEmpresaIdAsync(empresaId);

        await SetViewBagsAsync();

        return View(citas);
    }

    public async Task<IActionResult> Create()
    {
        var empresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault();

        var clientes = await _clienteService.GetClientesByEmpresaIdAsync(empresaId);
        ViewBag.Clientes = new SelectList(clientes.Select(c => new
        {
            Id = c.Id,
            NombreCompleto = $"{c.Nombre} {c.Apellido}"
        }), "Id", "NombreCompleto");

        var empleados = await _empleadoService.GetEmpleadosByEmpresaIdAsync(empresaId);
        ViewBag.Empleados = new SelectList(empleados.Select(e => new
        {
            Id = e.Id,
            NombreCompleto = $"{e.Nombre} {e.Apellido}"
        }), "Id", "NombreCompleto");

        var servicios = await _servicioService.GetServiciosByEmpresaIdAsync(empresaId);
        ViewBag.Servicios = servicios.Select(s => new
        {
            Id = s.Id,
            s.Nombre
        }).ToList();

        // Horarios disponibles
        var fechaActual = DateTime.Now.Date;
        var empleadoId = empleados.FirstOrDefault()?.Id ?? 0;

        var horariosDisponibles = empleadoId != 0
            ? await _citaService.GetAvailableSlotsAsync(fechaActual, empleadoId, 60)
            : new List<string>();

        ViewBag.HorariosDisponibles = horariosDisponibles;

        var productos = await _productoService.GetProductosByEmpresaIdAsync(empresaId);
        var model = new CitaViewModel
        {
            Fecha = DateTime.Now.Date,
            ServiciosSeleccionados = new List<int>(),
            ProductosSeleccionados = new List<ProductoSeleccionadoViewModel>(),
            ProductosDisponibles = productos.Select(p => new ProductoViewModel
            {
                ProductoId = p.ProductoId,
                NombreProducto = p.NombreProducto,
                Cantidad = 0,
                Seleccionado = false
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CitaViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Convertir el horario seleccionado a TimeSpan
            TimeSpan horarioSeleccionado;
            try
            {
                horarioSeleccionado = DateTime.Parse(model.HorarioSeleccionado).TimeOfDay;
            }
            catch (FormatException)
            {
                ModelState.AddModelError("HorarioSeleccionado", "El horario seleccionado no es válido.");
                await SetViewBagsAsync();
                return View(model);
            }

            var cita = new Cita
            {
                IdCliente = model.IdCliente,
                IdEmpleado = model.IdEmpleado,
                Fecha = model.Fecha.Add(horarioSeleccionado),
                IdEstadoCita = 1, // Pendiente por defecto
                IdEmpresa = (await GetEmpresaIdFromUser()).GetValueOrDefault(),
                PrecioEstimado = 0, // Calculado después
                DuracionEstimada = 0 
            };

            // Calcular duración y precio estimados según los servicios seleccionados
            foreach (var servicioId in model.ServiciosSeleccionados)
            {
                var servicio = await _servicioService.GetServicioByIdAsync(servicioId);
                if (servicio != null)
                {
                    cita.ServiciosXCita.Add(new ServiciosXcita
                    {
                        IdServicio = servicioId
                    });
                    cita.DuracionEstimada += servicio.DuracionEstimada; // Sumar duración del servicio
                    cita.PrecioEstimado += servicio.PrecioBase; // Sumar precio base del servicio
                }
            }

            // Calcular precio adicional según los productos seleccionados
            foreach (var producto in model.ProductosSeleccionados.Where(p => p.Seleccionado))
            {
                var productoInfo = await _productoService.GetProductoByIdAsync(producto.ProductoId);
                if (productoInfo != null)
                {
                    cita.ProductosXCita.Add(new ProductosXcita
                    {
                        IdProducto = producto.ProductoId,
                        Cantidad = producto.Cantidad
                    });
                    cita.PrecioEstimado += productoInfo.Precio * producto.Cantidad; // Calcular precio por cantidad
                }
            }

            // Validar disponibilidad de slots según la duración estimada
            var slotsDisponibles = await _citaService.GetAvailableSlotsAsync(model.Fecha, model.IdEmpleado, cita.DuracionEstimada);
            if (!slotsDisponibles.Contains(model.HorarioSeleccionado))
            {
                ModelState.AddModelError("HorarioSeleccionado", "El horario seleccionado no está disponible.");
                await SetViewBagsAsync();
                return View(model);
            }

            // Guardar cita
            await _citaService.AddCitaAsync(cita);

            // Reservar los slots correspondientes
            await _citaService.ReservarSlotsAsync(model.Fecha, model.IdEmpleado, horarioSeleccionado, cita.DuracionEstimada, cita.Id);
            await _citaService.EnviarMensajeWhatsAppAsync("543513444885");
            return RedirectToAction(nameof(Index));
        }

        await SetViewBagsAsync();
        return View(model);
    }
    public async Task<IActionResult> Edit(int id)
    {
        var cita = await _citaService.GetCitaByIdAsync(id);
        if (cita == null)
        {
            return NotFound();
        }

        // Configurar ViewBag.Servicios
        var servicios = await _servicioService.GetAllServiciosAsync();
        ViewBag.Servicios = new SelectList(await _servicioService.GetServiciosByEmpresaIdAsync(cita.IdEmpresa),"Id","Nombre");

        // Otros datos
        ViewBag.ClienteNombre = $"{cita.Cliente.Nombre} {cita.Cliente.Apellido}";
        ViewBag.EmpleadoNombre = $"{cita.Empleado.Nombre} {cita.Empleado.Apellido}";
        ViewBag.EstadosCita = new SelectList(await _estadoCitaService.GetAllEstadosAsync(), "Id", "Descripcion");
        ViewBag.HorariosDisponibles = new SelectList(await _citaService.GetAvailableSlotsAsync(cita.Fecha, cita.IdEmpleado, cita.DuracionEstimada), cita.Fecha.ToString("HH:mm"));

        var model = new CitaViewModel
        {
            Id = cita.Id,
            IdCliente = cita.IdCliente,
            IdEmpleado = cita.IdEmpleado,
            Fecha = cita.Fecha.Date,
            HorarioSeleccionado = cita.Fecha.ToString("HH:mm"),
            ServiciosSeleccionados = (await _servicioService.GetServiciosByCitaIdAsync(cita.Id))
            .Where(s => s.Seleccionado)
            .Select(s => s.Id)
            .ToList(),
        ProductosSeleccionados = await _productoService.GetProductosByCitaIdAsync(cita.Id),
        ProductosDisponibles = await _productoService.GetProductosByEmpresaIdAsync(cita.IdEmpresa)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CitaViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            TimeSpan horarioSeleccionado;
            try
            {
                horarioSeleccionado = DateTime.Parse(model.HorarioSeleccionado).TimeOfDay;
            }
            catch (FormatException)
            {
                ModelState.AddModelError("HorarioSeleccionado", "El horario seleccionado no es válido.");
                await SetViewBagsAsync();
                return View(model);
            }

            var cita = await _citaService.GetCitaByIdAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos editables
            cita.Fecha = model.Fecha.Add(horarioSeleccionado);
            cita.PrecioFinal = model.PrecioFinal;
            cita.IdEstadoCita= model.IdEstadoCita;
            // Recalcular duración y precio
            cita.DuracionEstimada = 0;
            cita.PrecioEstimado = 0;

            cita.ServiciosXCita.Clear();
            foreach (var servicioId in model.ServiciosSeleccionados)
            {
                var servicio = await _servicioService.GetServicioByIdAsync(servicioId);
                if (servicio != null)
                {
                    cita.ServiciosXCita.Add(new ServiciosXcita { IdServicio = servicioId });
                    cita.DuracionEstimada += servicio.DuracionEstimada;
                    cita.PrecioEstimado += servicio.PrecioBase;
                }
            }

            cita.ProductosXCita.Clear();
            foreach (var producto in model.ProductosSeleccionados.Where(p => p.Seleccionado))
            {
                var productoInfo = await _productoService.GetProductoByIdAsync(producto.ProductoId);
                if (productoInfo != null)
                {
                    cita.ProductosXCita.Add(new ProductosXcita
                    {
                        IdProducto = producto.ProductoId,
                        Cantidad = producto.Cantidad
                    });
                    cita.PrecioEstimado += productoInfo.Precio * producto.Cantidad;
                }
            }

            // Validar disponibilidad de slots
            var slotsDisponibles = await _citaService.GetAvailableSlotsAsync(model.Fecha, model.IdEmpleado, cita.DuracionEstimada);
            if (!slotsDisponibles.Contains(model.HorarioSeleccionado))
            {
                ModelState.AddModelError("HorarioSeleccionado", "El horario seleccionado no está disponible.");
                await SetViewBagsAsync();
                return View(model);
            }

            // Actualizar slots reservados
            await _citaService.CancelarReservasAsync(id);
            await _citaService.ReservarSlotsAsync(model.Fecha, model.IdEmpleado, horarioSeleccionado, cita.DuracionEstimada, cita.Id);

            // Guardar cambios en la cita
            await _citaService.UpdateCitaAsync(cita);

            return RedirectToAction(nameof(Index));
        }

        await SetViewBagsAsync();
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var cita = await _citaService.GetCitaByIdAsync(id);
        if (cita == null)
        {
            return NotFound();
        }

        return View(cita);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _citaService.DeleteCitaAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
