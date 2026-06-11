namespace Heleket.Abstractions;

/// <summary>
/// Root Heleket SDK client.
/// </summary>
public interface IHeleketClient
{
    /// <summary>
    /// Gets payment API operations.
    /// </summary>
    IHeleketPaymentsClient Payments { get; }

    /// <summary>
    /// Gets balance API operations.
    /// </summary>
    IHeleketBalanceClient Balance { get; }

    /// <summary>
    /// Gets webhook validation helpers.
    /// </summary>
    IHeleketWebhooksClient Webhooks { get; }
}
