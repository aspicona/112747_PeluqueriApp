using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeluqueriApp.Models;
using PeluqueriApp.Services;


public class ServicioController : Controller
{
    private readonly IServicioService _servicioService;
    private readonly IEmpresaService _empresaService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IInsumoService _insumoService;

    public ServicioController(IServicioService servicioService, IEmpresaService empresaService, UserManager<ApplicationUser> userManager, IInsumoService insumoService)
    {
        _servicioService = servicioService;
        _empresaService = empresaService;
        _userManager = userManager;
        _insumoService = insumoService;
    }

    private async Task<int?> GetEmpresaIdFromUser()
    {
        var user = await _userManager.GetUserAsync(User);
        return user?.IdEmpresa;
    }

    public async Task<IActionResult> Index()
    {
        var empresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault();

        if (empresaId == 0)
        {
            return Unauthorized();
        }

        var servicios = await _servicioService.GetServiciosByEmpresaIdAsync(empresaId);
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View(servicios);
    }

    // Crear servicio (GET)
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var empresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault();

        if (empresaId == 0)
        {
            return Unauthorized();
        }
        var insumos = await _insumoService.GetInsumosByEmpresaIdAsync(empresaId);
        var viewModel = new ServicioViewModel
        {
            InsumosAsignados = insumos.Select(i => new InsumoAsignadoViewModel
            {
                InsumoId = i.Id,
                NombreInsumo = i.Nombre,
                Seleccionado = false, // Por defecto no está seleccionado
                CantidadNecesaria = 0, // Valor inicial
                CostoUnitario=i.CostoUnitario
            }).ToList()
        };
        return View(viewModel);
    }

    // Crear servicio (POST)
    [HttpPost]
    public async Task<IActionResult> Create(ServicioViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Crear el objeto Servicio
            var servicio = new Servicio
            {
                EmpresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault(),
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                PrecioBase = model.PrecioBase,
                DuracionEstimada = model.DuracionEstimada,
                CostoInsumos= model.CostoInsumos
            };

            // Crear la lista de InsumosXservicio a partir del ViewModel
            var insumosXservicio = model.InsumosAsignados
                .Where(i => i.Seleccionado) // Filtrar solo los insumos seleccionados
                .Select(i => new InsumosXservicio
                {
                    IdInsumo = i.InsumoId,
                    CantidadNecesaria = i.CantidadNecesaria
                }).ToList();

            // Llamar al servicio para agregar el Servicio y los insumos relacionados
            await _servicioService.AddServicioAsync(servicio, insumosXservicio);

            return RedirectToAction("Index");
        }

        // Si el modelo no es válido, recargar los datos necesarios para el formulario
        await CargarDatosParaFormulario();
        return View(model);
    }


    // Editar servicio (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var servicio = await _servicioService.GetServicioByIdAsync(id);
        if (servicio == null)
        {
            return NotFound();
        }

        var model = new ServicioViewModel
        {
            Id = servicio.Id,
            Nombre = servicio.Nombre,
            Descripcion = servicio.Descripcion,
            PrecioBase = servicio.PrecioBase,
            DuracionEstimada = servicio.DuracionEstimada,
            InsumosAsignados = await _insumoService.GetInsumosByServicioIdAsync(id)
        };

        return View(model);
    }

    // Editar servicio (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ServicioViewModel model)
    {
        if (ModelState.IsValid)
        {
            var servicio = new Servicio
            {
                Id = model.Id,
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                PrecioBase = model.PrecioBase,
                DuracionEstimada = model.DuracionEstimada,
                EmpresaId = (await GetEmpresaIdFromUser()).GetValueOrDefault()
            };

            var nuevosInsumos = model.InsumosAsignados
                .Where(i => i.Seleccionado)
                .Select(i => new InsumosXservicio
                {
                    IdInsumo = i.InsumoId,
                    CantidadNecesaria = i.CantidadNecesaria
                }).ToList();

            await _servicioService.UpdateServicioAsync(servicio, nuevosInsumos);

            return RedirectToAction("Index");
        }

        // Si el modelo no es válido, volver a cargar los datos necesarios para el formulario
        await CargarDatosParaFormulario();
        return View(model);
    }



    // Eliminar servicio (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var servicio = await _servicioService.GetServicioByIdAsync(id);
        if (servicio == null)
        {
            return NotFound();
        }

        return View(servicio);
    }

    // Eliminar servicio (POST)
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _servicioService.DeleteServicioAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarDatosParaFormulario()
    {
        var insumos = await _insumoService.GetAllInsumosAsync();
        ViewBag.Insumos = insumos.Select(i => new InsumoAsignadoViewModel
        {
            InsumoId = i.Id,
            NombreInsumo = i.Nombre,
            Seleccionado = false, // Por defecto, los insumos no están seleccionados
            CantidadNecesaria = 0
        }).ToList();
    }

    [HttpGet]
    public async Task<IActionResult> CalcularDuracionTotal([FromQuery] List<int> servicioIds)
    {
        if (servicioIds == null || !servicioIds.Any())
        {
            return BadRequest("No se han proporcionado servicios.");
        }

        var duracionTotal = await _servicioService.CalcularDuracionTotalAsync(servicioIds);

        return Json(duracionTotal); // Devuelve la duración total en formato JSON
    }


}



