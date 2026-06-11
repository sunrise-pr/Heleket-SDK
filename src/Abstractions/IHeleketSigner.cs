namespace Heleket.Abstractions;

/// <summary>
/// Generates Heleket request and webhook signatures from exact JSON payloads.
/// </summary>
public interface IHeleketSigner
{
    /// <summary>
    /// Signs the supplied JSON string using the Heleket algorithm: MD5(base64(json) + apiKey).
    /// </summary>
    /// <param name="json">The exact JSON string to sign.</param>
    /// <param name="apiKey">The Heleket API key to append before hashing.</param>
    /// <returns>The lowercase hexadecimal MD5 signature.</returns>
    string SignJson(string json, string apiKey);
}
