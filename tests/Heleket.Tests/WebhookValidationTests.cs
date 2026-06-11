using Heleket.Abstractions;
using Heleket.Client;
using Heleket.Configuration;
using Heleket.DependencyInjection;
using Heleket.Signing;
using Heleket.Webhooks.Validation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Heleket.Tests;

public sealed class WebhookValidationTests
{
    private const string PaymentKey = "payment-secret";
    private const string PayoutKey = "payout-secret";

    [Fact]
    public void Validate_ValidPaymentWebhookPasses()
    {
        var validator = CreateValidator();
        var rawBody = SignedBody("{\"type\":\"payment\",\"order_id\":\"order_1\",\"amount\":\"10.00\",\"status\":\"paid\"}", PaymentKey);

        var isValid = validator.Validate(rawBody, PaymentKey);

        Assert.True(isValid);
    }

    [Fact]
    public void Validate_MissingSignFails()
    {
        var validator = CreateValidator();

        var isValid = validator.Validate("{\"type\":\"payment\",\"order_id\":\"order_1\"}", PaymentKey);

        Assert.False(isValid);
    }

    [Fact]
    public void Validate_ModifiedPayloadFails()
    {
        var validator = CreateValidator();
        var sign = new HeleketSigner().SignJson("{\"type\":\"payment\",\"order_id\":\"order_1\",\"amount\":\"10.00\"}", PaymentKey);
        var modifiedBody = "{\"type\":\"payment\",\"order_id\":\"order_1\",\"amount\":\"11.00\",\"sign\":\"" + sign + "\"}";

        var isValid = validator.Validate(modifiedBody, PaymentKey);

        Assert.False(isValid);
    }

    [Fact]
    public void Validate_WrongKeyFails()
    {
        var validator = CreateValidator();
        var rawBody = SignedBody("{\"type\":\"payment\",\"order_id\":\"order_1\"}", PaymentKey);

        var isValid = validator.Validate(rawBody, "wrong-secret");

        Assert.False(isValid);
    }

    [Fact]
    public void ClientWebhooks_ValidatePaymentUsesPaymentApiKey()
    {
        var client = CreateClient();
        var rawBody = SignedBody("{\"type\":\"payment\",\"order_id\":\"order_1\"}", PaymentKey);

        Assert.True(client.Webhooks.ValidatePayment(rawBody));
    }

    [Fact]
    public void ClientWebhooks_ValidatePayoutUsesPayoutApiKey()
    {
        var client = CreateClient();
        var rawBody = SignedBody("{\"type\":\"payout\",\"order_id\":\"order_1\"}", PayoutKey);

        Assert.True(client.Webhooks.ValidatePayout(rawBody));
        Assert.False(client.Webhooks.ValidatePayment(rawBody));
    }

    [Fact]
    public void ClientWebhooks_ValidatePayoutThrowsWhenPayoutApiKeyIsMissing()
    {
        var client = new HeleketClient(new HeleketOptions
        {
            MerchantId = "merchant-id",
            PaymentApiKey = PaymentKey
        });

        var exception = Assert.Throws<InvalidOperationException>(() => client.Webhooks.ValidatePayout("{}"));

        Assert.Contains(nameof(HeleketOptions.PayoutApiKey), exception.Message);
    }

    [Fact]
    public void Validate_SlashEscapedUrlPayloadPassesAfterNormalization()
    {
        var validator = CreateValidator();
        var normalizedJson = "{\"type\":\"payment\",\"url_callback\":\"https://example.com/hooks/heleket\",\"order_id\":\"order_1\"}";
        var sign = new HeleketSigner().SignJson(normalizedJson, PaymentKey);
        var rawBody = "{\"type\":\"payment\",\"url_callback\":\"https:\\/\\/example.com\\/hooks\\/heleket\",\"order_id\":\"order_1\",\"sign\":\"" + sign + "\"}";

        var isValid = validator.Validate(rawBody, PaymentKey);

        Assert.True(isValid);
    }

    [Fact]
    public void AddHeleket_RegistersConfiguredClientServices()
    {
        var services = new ServiceCollection();
        services.AddHeleket(options =>
        {
            options.MerchantId = "merchant-id";
            options.PaymentApiKey = PaymentKey;
            options.PayoutApiKey = PayoutKey;
        });

        Assert.Contains(services, service => service.ServiceType == typeof(IHeleketClient));
        Assert.Contains(services, service => service.ServiceType == typeof(IHeleketWebhookValidator));
    }

    private static IHeleketWebhookValidator CreateValidator()
    {
        return new HeleketWebhookValidator(new HeleketSigner());
    }

    private static HeleketClient CreateClient()
    {
        return new HeleketClient(new HeleketOptions
        {
            MerchantId = "merchant-id",
            PaymentApiKey = PaymentKey,
            PayoutApiKey = PayoutKey
        });
    }

    private static string SignedBody(string jsonWithoutSign, string apiKey)
    {
        var sign = new HeleketSigner().SignJson(jsonWithoutSign, apiKey);
        return jsonWithoutSign[..^1] + ",\"sign\":\"" + sign + "\"}";
    }
}
