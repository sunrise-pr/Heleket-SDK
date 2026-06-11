using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heleket.Services
{
    /// <summary>
    /// Legacy configured webhook verifier retained for compatibility.
    /// </summary>
    public interface IHeleketWebhookVerifier
    {
        /// <summary>
        /// Verifies the signature of a Heleket webhook request using the configured ApiPaymentKey.
        /// </summary>
        /// <param name="rawRequestBody">The raw JSON string received in the POST request body.</param>
        /// <returns>True if the signature is valid, False otherwise.</returns>
        bool VerifySignature(string? rawRequestBody);
    }
}
