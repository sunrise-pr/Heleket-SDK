using Heleket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heleket.Services
{
    /// <summary>
    /// Legacy Heleket API client retained for compatibility.
    /// </summary>
    public interface IHeleketApiClient
    {
        /// <summary>
        /// Creates or updates an invoice on Heleket.
        /// </summary>
        /// <param name="request">The invoice creation request details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response object indicating success/failure and potentially invoice details.</returns>
        Task<CreateInvoiceResponse> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns (status, payerAmountUsd) for the payment.
        /// payerAmountUsd — фактически оплаченная сумма в USD (null если недоступна).
        /// </summary>
        Task<(string? Status, decimal? PayerAmountUsd)> GetPaymentInfoAsync(string uuid, string orderId, CancellationToken cancellationToken = default);
        // Add other API methods here (e.g., GetInvoiceStatusAsync)
    }
}
