namespace Heleket.Abstractions;

/// <summary>
/// Stateless low-level Heleket webhook signature validator.
/// </summary>
public interface IHeleketWebhookValidator
{
    /// <summary>
    /// Validates a raw webhook JSON body with an explicit API key.
    /// </summary>
    /// <param name="rawBody">The raw JSON request body received from Heleket.</param>
    /// <param name="apiKey">The API key used to verify the signature.</param>
    /// <returns><see langword="true"/> when the signature is valid.</returns>
    bool Validate(string rawBody, string apiKey);
}
