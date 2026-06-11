using Heleket.Payments;
using Heleket.Webhooks.Models;
using Newtonsoft.Json;
using Xunit;

namespace Heleket.Tests;

public sealed class PaymentWebhookDtoTests
{
    [Fact]
    public void HeleketPaymentWebhook_DocShapedPayloadDeserializes()
    {
        var webhook = JsonConvert.DeserializeObject<HeleketPaymentWebhook>(PaymentWebhookJson);

        Assert.NotNull(webhook);
        Assert.Equal("payment", webhook!.Type);
        Assert.Equal("webhook-order", webhook.OrderId);
        Assert.Equal(PaymentStatus.Paid, webhook.Status);
        Assert.True(webhook.Status!.Value.IsSuccessfulFinal(webhook.IsFinal == true));
        Assert.Equal("abc123", webhook.Sign);
        Assert.Equal("field", webhook.ExtensionData!["future_field"].ToObject<string>());
    }

    private const string PaymentWebhookJson = """
        {
          "type": "payment",
          "uuid": "payment-uuid",
          "order_id": "webhook-order",
          "amount": "15.00",
          "payment_amount": "15.00",
          "payment_amount_usd": "15.00",
          "merchant_amount": "14.90",
          "commission": "0.10",
          "is_final": true,
          "status": "paid",
          "from": "sender-wallet",
          "wallet_address_uuid": "wallet-address-uuid",
          "network": "tron",
          "currency": "USDT",
          "payer_currency": "USDT",
          "additional_data": "metadata",
          "convert": {
            "to_currency": "USDT"
          },
          "txid": "txid-1",
          "sign": "abc123",
          "future_field": "field"
        }
        """;
}
