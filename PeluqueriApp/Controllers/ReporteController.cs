using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using Rotativa.AspNetCore;

public class ReporteController : Controller
{
    private readonly ICitaService _citaService;
    private readonly IInsumoService _insumoService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReporteController( ICitaService citaService, IInsumoService insumoService, UserManager<ApplicationUser> userManager)
    {
        _citaService = citaService;
        _insumoService = insumoService;
        _userManager = userManager;
    }

    private async Task<int?> GetEmpresaIdFromUser()
    {
        var user = await _userManager.GetUserAsync(User);
        return user?.IdEmpresa;
    }

    public async Task<IActionResult> Index()
    {        
        return View();
    }

    public async Task<IActionResult> ReporteIngresosPorCitas(DateTime startDate, DateTime endDate)
    {
        var ingresosPorFecha = await _citaService.ObtenerIngresosPorCitasAsync(startDate, endDate);

        // Preparar los datos
        var etiquetas = ingresosPorFecha.Select(i => i.Fecha).ToList();
        var ingresos = ingresosPorFecha.Select(i => i.Ingreso).ToList();

        // Pasar los datos al modelo de la vista
        ViewBag.Etiquetas = etiquetas;
        ViewBag.Ingresos = ingresos;

        return View(ingresosPorFecha);
    }
    public async Task<IActionResult> ExportarIngresosAPdf(DateTime startDate, DateTime endDate)
    {
        var ingresos = await _citaService.ObtenerIngresosPorCitasAsync(startDate, endDate);
        return new ViewAsPdf("ReporteIngresosPorCitasPdf", ingresos);
    }

    public async Task<IActionResult> ReporteServiciosRealizados(DateTime startDate, DateTime endDate)
{
    // Obtener el IdEmpresa del usuario logueado
    var empresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault();

    if (empresaId == 0)
    {
        return Unauthorized();
    }

    // Obtener los servicios realizados filtrados por empresa y fechas
    var serviciosRealizados = await _citaService.ObtenerServiciosRealizadosAsync(startDate, endDate, empresaId);

    // Preparar los datos para la vista
    ViewBag.Etiquetas = serviciosRealizados.Select(s => s.NombreServicio).ToList();
    ViewBag.Cantidades = serviciosRealizados.Select(s => s.Cantidad).ToList();

    return View(serviciosRealizados);
}

    //public async Task<IActionResult> ExportarServiciosAPdf(DateTime startDate, DateTime endDate)
    //{
    //    var servicios = await _citaService.ObtenerServiciosRealizadosAsync(startDate, endDate);
    //    return new ViewAsPdf("ReporteServiciosRealizadosPdf", servicios);
    //}
    public async Task<IActionResult> ReporteInsumosUtilizados(DateTime startDate, DateTime endDate)
    {
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        var empresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault();

        if (empresaId == 0)
        {
            return Unauthorized();
        }

        // Obtener servicios realizados con la cantidad de veces que cada servicio fue realizado
        var serviciosRealizados = await _citaService.ObtenerServiciosRealizadosAsync(startDate, endDate, empresaId);

        // Diccionario para agrupar insumos por unidad de medida
        var insumosPorUnidad = new Dictionary<string, Dictionary<string, decimal>>();

        foreach (var servicio in serviciosRealizados)
        {
            // Obtener los insumos necesarios para el servicio
            var insumosPorServicio = await _insumoService.GetInsumosByServicioIdAsync(servicio.Id);

            // Multiplicar la cantidad necesaria por la cantidad de veces que el servicio fue realizado
            foreach (var insumo in insumosPorServicio)
            {
                var cantidadTotal = insumo.CantidadNecesaria * servicio.Cantidad; // Multiplicar por la cantidad de veces que se realizó el servicio

                if (!insumosPorUnidad.ContainsKey(insumo.UnidadDeMedida))
                {
                    insumosPorUnidad[insumo.UnidadDeMedida] = new Dictionary<string, decimal>();
                }

                if (insumosPorUnidad[insumo.UnidadDeMedida].ContainsKey(insumo.NombreInsumo))
                {
                    insumosPorUnidad[insumo.UnidadDeMedida][insumo.NombreInsumo] += cantidadTotal;
                }
                else
                {
                    insumosPorUnidad[insumo.UnidadDeMedida][insumo.NombreInsumo] = cantidadTotal;
                }
            }
        }

        // Pasar el diccionario de insumos a la vista
        ViewBag.InsumosPorUnidad = insumosPorUnidad;

        return View();
    }
}




