﻿using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;

namespace PeluqueriApp.Services
{

    public interface IMercadoPagoService
    {
        Task<Payment> CreatePaymentAsync(decimal amount, string description, string customerId, string cardToken, string securityCode, string email);
        Task<string> CreateCustomerAsync(string email);
        Task<string> CreateCardAsync(string customerId, string cardToken);
        Task<string> GetCustomerIdByEmailAsync(string email);
        Task<string> GenerateCardTokenAsync(string cardNumber, int expirationMonth, int expirationYear, string cardholderName, string securityCode);
    }

}
