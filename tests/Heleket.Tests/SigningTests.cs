using System.Text.Json.Serialization;
using Heleket.Services;
using Xunit;

namespace Heleket.Tests;

public sealed class SigningTests
{
    [Theory]
    [InlineData("", "5ebe2294ecd0e0f08eab7690d2a6ee69")]
    [InlineData("{}", "847494ce9311176ff4da7f7ac064a396")]
    [InlineData("{\"amount\":\"50.00\",\"currency\":\"USD\",\"order_id\":\"order_1\"}", "2a358d760544186041e58615dea17f1a")]
    public void SignJson_UsesBase64JsonPlusApiKeyMd5(string json, string expected)
    {
        var signer = new HeleketSigner();

        var actual = signer.SignJson(json, "secret");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GenerateRequestSignature_UsesSnakeCaseAndOmitsNulls()
    {
        var request = new SigningRequest
        {
            Amount = "50.00",
            Currency = "USD",
            OrderId = "order_1",
            UrlCallback = null
        };

        var signature = SignatureGenerator.GenerateRequestSignature(request, "secret");

        Assert.Equal("2a358d760544186041e58615dea17f1a", signature);
    }

    private sealed class SigningRequest
    {
        [JsonPropertyName("amount")]
        public required string Amount { get; init; }

        [JsonPropertyName("currency")]
        public required string Currency { get; init; }

        [JsonPropertyName("order_id")]
        public required string OrderId { get; init; }

        [JsonPropertyName("url_callback")]
        public string? UrlCallback { get; init; }
    }
}
