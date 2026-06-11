using Flurl.Http.Configuration;
using Heleket.Abstractions;
using Heleket.Configuration;
using Heleket.Internal;
using Heleket.Signing;
using Heleket.Webhooks.Validation;

namespace Heleket.Client;

/// <summary>
/// Default implementation of the root Heleket SDK client.
/// </summary>
public sealed class HeleketClient : IHeleketClient
{
    private static readonly IFlurlClientCache DefaultClientCache = new FlurlClientCache();

    /// <summary>
    /// Creates a client using Flurl for HTTP requests.
    /// </summary>
    /// <param name="options">The Heleket SDK options.</param>
    public HeleketClient(HeleketOptions options)
        : this(options, DefaultClientCache, new HeleketSigner(), new HeleketWebhookValidator(new HeleketSigner()))
    {
    }

    internal HeleketClient(
        HeleketOptions options,
        IFlurlClientCache clientCache,
        IHeleketSigner signer,
        IHeleketWebhookValidator webhookValidator)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(clientCache);
        options.Validate();

        var sender = new HeleketHttpSender(clientCache, options, signer);
        Payments = new HeleketPaymentsClient(sender);
        Balance = new HeleketBalanceClient(sender);
        Webhooks = new HeleketWebhooksClient(options, webhookValidator);
    }

    /// <inheritdoc />
    public IHeleketPaymentsClient Payments { get; }

    /// <inheritdoc />
    public IHeleketBalanceClient Balance { get; }

    /// <inheritdoc />
    public IHeleketWebhooksClient Webhooks { get; }
}
