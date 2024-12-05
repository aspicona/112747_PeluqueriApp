using PeluqueriApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using PeluqueriApp.Models;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using System.Globalization;

namespace PeluqueriApp.Controllers
{

    public class MercadoPagoController : Controller
    {
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly IConfiguration _configuration;
        private readonly ICitaService _citaService;

        public MercadoPagoController(IMercadoPagoService mercadoPagoService, IConfiguration configuration, ICitaService citaService)
        {
            _mercadoPagoService = mercadoPagoService ?? throw new ArgumentNullException(nameof(mercadoPagoService));
            _configuration = configuration;
            _citaService = citaService;
        }

        [HttpGet("customerFind")]
        public async Task<IActionResult> GetCustomerId([FromQuery] string email)
        {
            try
            {
                var customerId = await _mercadoPagoService.GetCustomerIdByEmailAsync(email);

                if (customerId != null)
                {
                    return Ok(customerId);
                }
                else
                {
                    return NotFound($"No se encontró el customer_id para el email: {email}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el customer_id: {ex.Message}");
            }
        }

        
        public async Task<IActionResult> CreatePayment(int id)
        {
            Cita cita = await _citaService.GetCitaByIdAsync(id);
            //decimal formattedPrice = decimal.Parse(cita.PrecioFinal.ToString("F2", CultureInfo.InvariantCulture));
            var accessToken = _configuration["MercadoPago:AccessToken"];
            MercadoPagoConfig.AccessToken = accessToken;

            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Id = "1",
                    Title = cita.Detalle,
                    //Description = cita.Detalle,
                    CurrencyId = "ARS",
                    Quantity = 1,
                    UnitPrice = cita.PrecioFinal
                }
            },
                Payer = new PreferencePayerRequest
                {
                    Email = ""
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://localhost:7230/Pago/Confirmacion",
                    Failure = "https://localhost:7230/Pago/Error"
                },
                AutoReturn = "approved"
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return Redirect(preference.SandboxInitPoint); // Para pruebas
        }

        [HttpGet("Success")]
        public IActionResult Success([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(new { Message = "Pago Exitoso", paymentResponse });
        }

        [HttpGet("Failure")]
        public IActionResult Failure([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(new { Message = "Pago Fallido", paymentResponse });
        }
    }


}