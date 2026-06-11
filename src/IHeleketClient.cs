namespace Heleket;

/// <summary>
/// Root Heleket SDK client.
/// </summary>
public interface IHeleketClient
{
    /// <summary>
    /// Gets payment API operations.
    /// </summary>
    Heleket.Payments.IHeleketPaymentsClient Payments { get; }

    /// <summary>
    /// Gets webhook validation helpers.
    /// </summary>
    IHeleketWebhooksClient Webhooks { get; }
}
