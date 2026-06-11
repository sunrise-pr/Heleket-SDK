using Heleket.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Heleket.Services
{
    internal class HeleketWebhookVerifier : IHeleketWebhookVerifier
    {
        private readonly HeleketOptions _options;
        private readonly IHeleketWebhookValidator _validator;
        private readonly ILogger<HeleketWebhookVerifier> _logger;

        public HeleketWebhookVerifier(
            IOptions<HeleketOptions> options,
            IHeleketWebhookValidator validator,
            ILogger<HeleketWebhookVerifier> logger)
        {
            _options = options.Value;
            _validator = validator;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_options.MerchantId))
            {
                throw new InvalidOperationException($"Heleket {nameof(_options.MerchantId)} is not configured. Webhook verification cannot proceed.");
            }
        }

        public bool VerifySignature(string? rawRequestBody)
        {
            if (string.IsNullOrWhiteSpace(rawRequestBody))
            {
                _logger?.LogWarning("Webhook verification failed: Request body is empty.");
                return false;
            }

            var isValid = _validator.Validate(rawRequestBody, _options.PaymentApiKey!);
            if (!isValid)
            {
                _logger?.LogWarning("Webhook verification failed: Signature mismatch.");
            }

            return isValid;
        }
    }
}
