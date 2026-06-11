using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Heleket.Options;
using Heleket.Services;

namespace Heleket.Internal;

internal sealed class HeleketHttpSender
{
    private readonly HttpClient _httpClient;
    private readonly HeleketOptions _options;
    private readonly IHeleketSigner _signer;

    public HeleketHttpSender(HttpClient httpClient, HeleketOptions options, IHeleketSigner signer)
    {
        _httpClient = httpClient;
        _options = options;
        _signer = signer;
    }

    public async Task<HeleketResponse<T>> PostPaymentAsync<T>(
        string path,
        object request,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<T>(path, request, _options.PaymentApiKey!, cancellationToken).ConfigureAwait(false);
    }

    private async Task<HeleketResponse<T>> PostAsync<T>(
        string path,
        object request,
        string apiKey,
        CancellationToken cancellationToken)
    {
        var json = HeleketJson.Serialize(request);
        using var message = new HttpRequestMessage(HttpMethod.Post, BuildUri(path));
        message.Headers.TryAddWithoutValidation("merchant", _options.MerchantId);
        message.Headers.TryAddWithoutValidation("sign", _signer.SignJson(json, apiKey));
        message.Content = new StringContent(json, Encoding.UTF8, "application/json");
        message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        using var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new HeleketApiException(response.StatusCode, responseBody);
        }

        var envelope = JsonSerializer.Deserialize<HeleketResponse<T>>(responseBody, HeleketJson.Options);
        return envelope ?? throw new HeleketApiException(response.StatusCode, responseBody);
    }

    private Uri BuildUri(string path)
    {
        var baseUrl = _options.BaseUrl.EndsWith("/", StringComparison.Ordinal)
            ? _options.BaseUrl
            : _options.BaseUrl + "/";

        return new Uri(new Uri(baseUrl), path);
    }
}
