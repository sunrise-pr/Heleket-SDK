using System.Net;
using System.Reflection;
using Heleket;
using Heleket.Models;
using Heleket.Options;
using Heleket.Payments;
using Heleket.Services;
using Xunit;

namespace Heleket.Tests;

public sealed class PaymentClientTests
{
    [Fact]
    public async Task CreateAsync_SendsPostToPaymentEndpoint()
    {
        var handler = new CaptureHandler("{\"state\":0,\"result\":{\"uuid\":\"payment-uuid\",\"order_id\":\"order_1\",\"url\":\"https://pay.example\"}}");
        var client = CreateClient(handler);

        var response = await client.Payments.CreateAsync(new CreatePaymentRequest
        {
            Amount = "50.00",
            Currency = "USD",
            OrderId = "order_1",
            UrlCallback = "https://example.com/hooks/heleket"
        });

        Assert.True(response.IsSuccess);
        Assert.Equal(HttpMethod.Post, handler.Request!.Method);
        Assert.Equal("/v1/payment", handler.Request.RequestUri!.AbsolutePath);
        Assert.Contains("\"url_callback\":\"https://example.com/hooks/heleket\"", handler.Body);
    }

    [Fact]
    public async Task GetInfoAsync_SendsOrderIdSnakeCase()
    {
        var handler = new CaptureHandler("{\"state\":0,\"result\":{\"order_id\":\"order_1\",\"status\":\"paid\"}}");
        var client = CreateClient(handler);

        await client.Payments.GetInfoAsync(new GetPaymentInfoRequest
        {
            OrderId = "order_1"
        });

        Assert.Equal("/v1/payment/info", handler.Request!.RequestUri!.AbsolutePath);
        Assert.Contains("\"order_id\":\"order_1\"", handler.Body);
        Assert.DoesNotContain("orderId", handler.Body);
    }

    [Fact]
    public async Task RefundAsync_SupportsUuidAndOrderId()
    {
        var handler = new CaptureHandler("{\"state\":0,\"result\":{\"uuid\":\"payment-uuid\",\"order_id\":\"order_1\",\"status\":\"refund_process\"}}");
        var client = CreateClient(handler);

        var response = await client.Payments.RefundAsync(new RefundPaymentRequest
        {
            Uuid = "payment-uuid",
            OrderId = "order_1",
            Address = "wallet-address",
            IsSubtract = true,
            Amount = "10.00"
        });

        Assert.True(response.IsSuccess);
        Assert.Equal("/v1/payment/refund", handler.Request!.RequestUri!.AbsolutePath);
        Assert.Contains("\"uuid\":\"payment-uuid\"", handler.Body);
        Assert.Contains("\"order_id\":\"order_1\"", handler.Body);
        Assert.Contains("\"is_subtract\":true", handler.Body);
    }

    [Fact]
    public async Task ApiErrors_ArePreservedInStructuredResponse()
    {
        var handler = new CaptureHandler("{\"state\":1,\"message\":\"Validation failed\",\"errors\":{\"order_id\":[\"validation.required\"]}}");
        var client = CreateClient(handler);

        var response = await client.Payments.GetInfoAsync(new GetPaymentInfoRequest());

        Assert.False(response.IsSuccess);
        Assert.Equal(1, response.State);
        Assert.Equal("Validation failed", response.Message);
        Assert.Equal("validation.required", response.Errors!["order_id"][0]);
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

    private static HeleketClient CreateClient(CaptureHandler handler)
    {
        return new HeleketClient(
            new HeleketOptions
            {
                MerchantId = "merchant-id",
                PaymentApiKey = "payment-secret",
                PayoutApiKey = "payout-secret",
                BaseUrl = "https://api.test/v1/"
            },
            new HttpClient(handler));
    }

    private sealed class CaptureHandler : HttpMessageHandler
    {
        private readonly string _responseBody;

        public CaptureHandler(string responseBody)
        {
            _responseBody = responseBody;
        }

        public HttpRequestMessage? Request { get; private set; }

        public string? Body { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            Body = request.Content is null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_responseBody)
            };
        }
    }
}
