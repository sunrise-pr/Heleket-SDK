using System.Reflection;
using Flurl.Http.Testing;
using Heleket;
using Heleket.Models;
using Heleket.Options;
using Heleket.Payments;
using Heleket.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Heleket.Tests;

public sealed class PaymentClientTests
{
    [Fact]
    public async Task CreateAsync_SendsDocumentedSnakeCaseBodyAndHeaders()
    {
        using var httpTest = new HttpTest();
        httpTest.RespondWith(CreatePaymentSuccessJson);
        var client = CreateClient();

        var response = await client.Payments.CreateAsync(new CreatePaymentRequest
        {
            Amount = "15",
            Currency = "USD",
            OrderId = "1",
            Network = "tron",
            UrlCallback = "https://example.com/hooks/heleket",
            Currencies =
            [
                new PaymentCurrency { Currency = "USDT", Network = "tron" }
            ],
            ExceptCurrencies =
            [
                new PaymentCurrency { Currency = "BTC" }
            ]
        });

        const string expectedBody = "{\"amount\":\"15\",\"currency\":\"USD\",\"order_id\":\"1\",\"network\":\"tron\",\"url_callback\":\"https://example.com/hooks/heleket\",\"currencies\":[{\"currency\":\"USDT\",\"network\":\"tron\"}],\"except_currencies\":[{\"currency\":\"BTC\"}]}";

        Assert.True(response.IsSuccess);
        Assert.Equal("1ec87133-b22d-4643-988f-cac29a6ac85d", response.Result!.Uuid);
        Assert.Equal("78.45392451", response.Result.PayerAmountExchangeRate);
        Assert.Null(response.Result.Comments);

        var call = httpTest.CallLog.Single();
        Assert.Equal(expectedBody, call.RequestBody);
        httpTest.ShouldHaveCalled("https://api.test/v1/payment")
            .WithVerb(HttpMethod.Post)
            .WithHeader("merchant", "merchant-id")
            .WithHeader("sign", new HeleketSigner().SignJson(expectedBody, "payment-secret"))
            .WithHeader("Content-Type", "application/json*")
            .Times(1);
    }

    [Fact]
    public async Task GetInfoAsync_SendsDocumentedSnakeCaseBodyAndHeaders()
    {
        using var httpTest = new HttpTest();
        httpTest.RespondWith(PaymentInfoSuccessJson);
        var client = CreateClient();

        var response = await client.Payments.GetInfoAsync(new GetPaymentInfoRequest
        {
            Uuid = "70b8db5c-b952-406d-af26-4e1c34c27f15"
        });

        const string expectedBody = "{\"uuid\":\"70b8db5c-b952-406d-af26-4e1c34c27f15\"}";

        Assert.True(response.IsSuccess);
        Assert.Equal("cancel", response.Result!.PaymentStatus);
        Assert.Equal("15.43500000", response.Result.MerchantAmount);
        Assert.Null(response.Result.From);
        Assert.Null(response.Result.Txid);

        var call = httpTest.CallLog.Single();
        Assert.Equal(expectedBody, call.RequestBody);
        httpTest.ShouldHaveCalled("https://api.test/v1/payment/info")
            .WithVerb(HttpMethod.Post)
            .WithHeader("merchant", "merchant-id")
            .WithHeader("sign", new HeleketSigner().SignJson(expectedBody, "payment-secret"))
            .WithHeader("Content-Type", "application/json*")
            .Times(1);
    }

    [Fact]
    public async Task RefundAsync_SendsDocumentedSnakeCaseBodyAndHandlesArrayResult()
    {
        using var httpTest = new HttpTest();
        httpTest.RespondWith(RefundSuccessJson);
        var client = CreateClient();

        var response = await client.Payments.RefundAsync(new RefundPaymentRequest
        {
            Uuid = "8b03432e-385b-4670-8d06-064591096795",
            Address = "TDD97yguPESTpcrJMqU6h2ozZbibv4Vaqm",
            IsSubtract = true,
            Amount = "10.00"
        });

        const string expectedBody = "{\"uuid\":\"8b03432e-385b-4670-8d06-064591096795\",\"address\":\"TDD97yguPESTpcrJMqU6h2ozZbibv4Vaqm\",\"is_subtract\":true,\"amount\":\"10.00\"}";

        Assert.True(response.IsSuccess);
        Assert.Empty(response.Result!.Items);

        var call = httpTest.CallLog.Single();
        Assert.Equal(expectedBody, call.RequestBody);
        httpTest.ShouldHaveCalled("https://api.test/v1/payment/refund")
            .WithVerb(HttpMethod.Post)
            .WithHeader("merchant", "merchant-id")
            .WithHeader("sign", new HeleketSigner().SignJson(expectedBody, "payment-secret"))
            .WithHeader("Content-Type", "application/json*")
            .Times(1);
    }

    [Fact]
    public async Task ApiErrorEnvelope_IsReturnedForDocumentedValidationError()
    {
        using var httpTest = new HttpTest();
        httpTest.RespondWith(ApiErrorEnvelopeJson, status: 422);
        var client = CreateClient();

        var response = await client.Payments.GetInfoAsync(new GetPaymentInfoRequest());

        Assert.False(response.IsSuccess);
        Assert.Equal(1, response.State);
        Assert.Equal("validation.required_without", response.Errors!["uuid"][0]);
        Assert.Equal("validation.required_without", response.Errors["order_id"][0]);
    }

    [Fact]
    public void PaymentResponse_DeserializesNullOptionalFieldsAndUnknownFields()
    {
        var response = JsonConvert.DeserializeObject<HeleketResponse<HeleketPayment>>(PaymentInfoWithExtraFieldJson);

        Assert.NotNull(response);
        Assert.True(response!.IsSuccess);
        Assert.Null(response.Result!.Comments);
        Assert.Null(response.Result.From);
        Assert.Null(response.Result.Txid);
        Assert.Equal("value", response.Result.ExtensionData!["future_field"].Value<string>());
    }

    [Fact]
    public void PublicPaymentDtos_DoNotExposeDoubleAmount()
    {
        var dtoTypes = new[]
        {
            typeof(CreatePaymentRequest),
            typeof(HeleketPayment),
            typeof(RefundPaymentRequest),
            typeof(RefundPaymentResponse)
        };

        var doubleAmountProperties = dtoTypes
            .SelectMany(type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            .Where(property => property.Name.Contains("Amount", StringComparison.OrdinalIgnoreCase))
            .Where(property => property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            .ToArray();

        Assert.Empty(doubleAmountProperties);
    }

    [Fact]
    public void LegacyApiClientMethodsRemainAvailable()
    {
        Assert.NotNull(typeof(IHeleketApiClient).GetMethod(nameof(IHeleketApiClient.CreateInvoiceAsync)));
        Assert.NotNull(typeof(IHeleketApiClient).GetMethod(nameof(IHeleketApiClient.GetPaymentInfoAsync)));
        Assert.NotNull(typeof(CreateInvoiceRequest).GetConstructor(new[] { typeof(double), typeof(string), typeof(string) }));
    }

    private static HeleketClient CreateClient()
    {
        return new HeleketClient(new HeleketOptions
        {
            MerchantId = "merchant-id",
            PaymentApiKey = "payment-secret",
            PayoutApiKey = "payout-secret",
            BaseUrl = "https://api.test/v1/"
        });
    }

    private const string CreatePaymentSuccessJson = """
        {
          "state": 0,
          "result": {
            "uuid": "1ec87133-b22d-4643-988f-cac29a6ac85d",
            "order_id": "3",
            "amount": "20000.00",
            "payment_amount": null,
            "payment_amount_usd": "0.00",
            "payer_amount": "254.92",
            "payer_amount_exchange_rate": "78.45392451",
            "discount_percent": 0,
            "discount": "0.00",
            "payer_currency": "USDT",
            "currency": "RUB",
            "comments": null,
            "merchant_amount": "249.82816502",
            "network": "bsc",
            "address": "0x2b...",
            "from": null,
            "txid": null,
            "payment_status": "check",
            "url": "https://pay.heleket.com/pay/1ec87133-b22d-4643-988f-cac29a6ac85d",
            "expired_at": 1753202502,
            "status": "check",
            "is_final": false,
            "additional_data": null,
            "created_at": "2025-07-22T18:41:42+03:00",
            "updated_at": "2025-07-22T18:42:31+03:00",
            "commission": "5.09853397",
            "address_qr_code": "data:image/png;base64 ..."
          }
        }
        """;

    private const string PaymentInfoSuccessJson = """
        {
          "state": 0,
          "result": {
            "uuid": "70b8db5c-b952-406d-af26-4e1c34c27f15",
            "order_id": "65bbe87b4098c17a31cff3e71e515243",
            "amount": "15.00",
            "payment_amount": "0.00",
            "payer_amount": "15.75",
            "discount_percent": -5,
            "discount": "-0.75",
            "payer_currency": "USDT",
            "currency": "USDT",
            "comments": null,
            "merchant_amount": "15.43500000",
            "network": "tron",
            "address": "TXhfYSWt2oKRrHAJVJeYRuit6ZzKuoEKXj",
            "from": null,
            "txid": null,
            "payment_status": "cancel",
            "url": "https://pay.heleket.com/pay/70b8db5c-b952-406d-af26-4e1c34c27f15",
            "expired_at": 1689099831,
            "status": "cancel",
            "is_final": true,
            "additional_data": null,
            "created_at": "2023-07-11T20:23:52+03:00",
            "updated_at": "2023-07-11T21:24:17+03:00"
          }
        }
        """;

    private const string RefundSuccessJson = """
        {
          "state": 0,
          "result": []
        }
        """;

    private const string ApiErrorEnvelopeJson = """
        {
          "state": 1,
          "errors": {
            "uuid": ["validation.required_without"],
            "order_id": ["validation.required_without"]
          }
        }
        """;

    private const string PaymentInfoWithExtraFieldJson = """
        {
          "state": 0,
          "result": {
            "uuid": "70b8db5c-b952-406d-af26-4e1c34c27f15",
            "order_id": "65bbe87b4098c17a31cff3e71e515243",
            "amount": "15.00",
            "payment_amount": "0.00",
            "payer_amount": "15.75",
            "discount_percent": -5,
            "discount": "-0.75",
            "payer_currency": "USDT",
            "currency": "USDT",
            "comments": null,
            "merchant_amount": "15.43500000",
            "network": "tron",
            "address": "TXhfYSWt2oKRrHAJVJeYRuit6ZzKuoEKXj",
            "from": null,
            "txid": null,
            "payment_status": "cancel",
            "url": "https://pay.heleket.com/pay/70b8db5c-b952-406d-af26-4e1c34c27f15",
            "expired_at": 1689099831,
            "status": "cancel",
            "is_final": true,
            "additional_data": null,
            "created_at": "2023-07-11T20:23:52+03:00",
            "updated_at": "2023-07-11T21:24:17+03:00",
            "future_field": "value"
          }
        }
        """;
}
