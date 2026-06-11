using Heleket.Options;
using Heleket.Services;

namespace Heleket;

internal sealed class HeleketWebhooksClient : IHeleketWebhooksClient
{
    private readonly HeleketOptions _options;
    private readonly IHeleketWebhookValidator _validator;

    public HeleketWebhooksClient(HeleketOptions options, IHeleketWebhookValidator validator)
    {
        _options = options;
        _validator = validator;
    }

    /// <inheritdoc />
    public bool ValidatePayment(string rawBody)
    {
        return _validator.Validate(rawBody, _options.PaymentApiKey!);
    }

    /// <inheritdoc />
    public bool ValidatePayout(string rawBody)
    {
        if (string.IsNullOrWhiteSpace(_options.PayoutApiKey))
        {
            throw new InvalidOperationException($"{nameof(HeleketOptions.PayoutApiKey)} is not configured.");
        }

        return _validator.Validate(rawBody, _options.PayoutApiKey);
    }
}
