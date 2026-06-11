using Heleket.Options;
using Heleket.Internal;
using Heleket.Payments;
using Heleket.Services;

namespace Heleket;

/// <summary>
/// Default implementation of the root Heleket SDK client.
/// </summary>
public sealed class HeleketClient : IHeleketClient
{
    /// <summary>
    /// Creates a client using a new <see cref="HttpClient"/> instance.
    /// </summary>
    /// <param name="options">The Heleket SDK options.</param>
    public HeleketClient(HeleketOptions options)
        : this(options, new HttpClient())
    {
    }

    /// <summary>
    /// Creates a client using the supplied <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="options">The Heleket SDK options.</param>
    /// <param name="httpClient">The HTTP client used to send API requests.</param>
    public HeleketClient(HeleketOptions options, HttpClient httpClient)
        : this(options, httpClient, new HeleketSigner(), new HeleketWebhookValidator(new HeleketSigner()))
    {
    }

    internal HeleketClient(HeleketOptions options, HttpClient httpClient, IHeleketSigner signer, IHeleketWebhookValidator webhookValidator)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(httpClient);
        options.Validate();

        var sender = new HeleketHttpSender(httpClient, options, signer);
        Payments = new HeleketPaymentsClient(sender);
        Webhooks = new HeleketWebhooksClient(options, webhookValidator);
    }

    /// <inheritdoc />
    public IHeleketPaymentsClient Payments { get; }

    /// <inheritdoc />
    public IHeleketWebhooksClient Webhooks { get; }
}
