using Heleket.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Heleket.Signing;

/// <summary>
/// Default Heleket signature generator.
/// </summary>
public sealed class HeleketSigner : IHeleketSigner
{
    /// <inheritdoc />
    public string SignJson(string json, string apiKey)
    {
        return SignJsonValue(json, apiKey);
    }

    internal static string SignJsonValue(string json, string apiKey)
    {
        ArgumentNullException.ThrowIfNull(json);
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var base64Json = Convert.ToBase64String(jsonBytes);
        var bytesToHash = Encoding.UTF8.GetBytes(base64Json + apiKey);
        var hashBytes = MD5.HashData(bytesToHash);

        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
