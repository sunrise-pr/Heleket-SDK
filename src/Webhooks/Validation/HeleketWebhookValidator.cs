using System.Security.Cryptography;
using Heleket.Abstractions;
using Heleket.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heleket.Webhooks.Validation;

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
        JObject payloadWithoutSign;

        try
        {
            var root = JObject.Parse(rawBody);
            if (!TryExtractPayload(root, out payloadWithoutSign, out receivedSign))
            {
                return false;
            }
        }
        catch (JsonReaderException)
        {
            return false;
        }

        var jsonWithoutSign = HeleketJson.Serialize(payloadWithoutSign);
        var expectedSign = _signer.SignJson(jsonWithoutSign, apiKey);

        return FixedTimeEqualsHex(expectedSign, receivedSign);
    }

    private static bool TryExtractPayload(
        JObject root,
        out JObject payloadWithoutSign,
        out string receivedSign)
    {
        payloadWithoutSign = new JObject();
        receivedSign = string.Empty;
        var signFound = false;

        foreach (var property in root.Properties())
        {
            if (property.Name == "sign")
            {
                if (property.Value.Type != JTokenType.String)
                {
                    return false;
                }

                receivedSign = property.Value.Value<string>() ?? string.Empty;
                signFound = !string.IsNullOrWhiteSpace(receivedSign);
                continue;
            }

            payloadWithoutSign[property.Name] = property.Value.DeepClone();
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
