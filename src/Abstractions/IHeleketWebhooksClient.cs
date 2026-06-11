namespace Heleket.Abstractions;

/// <summary>
/// Provides configured webhook validation helpers.
/// </summary>
public interface IHeleketWebhooksClient
{
    /// <summary>
    /// Validates a payment webhook using the configured payment API key.
    /// </summary>
    /// <param name="rawBody">The raw webhook JSON request body.</param>
    /// <returns><see langword="true"/> when the webhook signature is valid.</returns>
    bool ValidatePayment(string rawBody);

    /// <summary>
    /// Validates a payout webhook using the configured payout API key.
    /// </summary>
    /// <param name="rawBody">The raw webhook JSON request body.</param>
    /// <returns><see langword="true"/> when the webhook signature is valid.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no payout API key is configured.</exception>
    bool ValidatePayout(string rawBody);
}
