using System.Security.Cryptography;
using System.Text.Json;
using Heleket.Internal;

namespace Heleket.Services;

/// <summary>
/// Default low-level Heleket webhook signature validator.
/// </summary>
public sealed class HeleketWebhookValidator : IHeleketWebhookValidator
{
    private readonly IHeleketSigner _signer;

    /// <summary>
    /// Creates a validator using the supplied signer.
    /// </summary>
    /// <param name="signer">The signature generator used to calculate expected signatures.</param>
    public HeleketWebhookValidator(IHeleketSigner signer)
    {
        _signer = signer;
    }

    /// <inheritdoc />
    public bool Validate(string rawBody, string apiKey)
    {
        if (string.IsNullOrWhiteSpace(rawBody) || string.IsNullOrWhiteSpace(apiKey))
        {
            return false;
        }

        string receivedSign;
        var payloadWithoutSign = new Dictionary<string, JsonElement>();

        try
        {
            using var document = JsonDocument.Parse(rawBody);
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                return false;
            }

            if (!TryExtractPayload(document.RootElement, payloadWithoutSign, out receivedSign))
            {
                return false;
            }
        }
        catch (JsonException)
        {
            return false;
        }

        var jsonWithoutSign = HeleketJson.Serialize(payloadWithoutSign);
        var expectedSign = _signer.SignJson(jsonWithoutSign, apiKey);

        return FixedTimeEqualsHex(expectedSign, receivedSign);
    }

    private static bool TryExtractPayload(
        JsonElement root,
        Dictionary<string, JsonElement> payloadWithoutSign,
        out string receivedSign)
    {
        receivedSign = string.Empty;
        var signFound = false;

        foreach (var property in root.EnumerateObject())
        {
            if (property.NameEquals("sign"))
            {
                if (property.Value.ValueKind != JsonValueKind.String)
                {
                    return false;
                }

                receivedSign = property.Value.GetString() ?? string.Empty;
                signFound = !string.IsNullOrWhiteSpace(receivedSign);
                continue;
            }

            payloadWithoutSign[property.Name] = property.Value.Clone();
        }

        return signFound;
    }

    private static bool FixedTimeEqualsHex(string expectedHex, string receivedHex)
    {
        try
        {
            if (expectedHex.Length != receivedHex.Length || expectedHex.Length % 2 != 0)
            {
                return false;
            }

            var expectedBytes = Convert.FromHexString(expectedHex);
            var receivedBytes = Convert.FromHexString(receivedHex);
            return CryptographicOperations.FixedTimeEquals(expectedBytes, receivedBytes);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
