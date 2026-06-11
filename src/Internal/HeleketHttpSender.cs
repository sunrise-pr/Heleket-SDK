using Flurl.Http;
using Flurl.Http.Configuration;
using Heleket.Abstractions;
using Heleket.Common;
using Heleket.Configuration;
using System.Net;

namespace Heleket.Internal;

internal sealed class HeleketHttpSender
{
    private readonly IFlurlClientCache _clientCache;
    private readonly HeleketOptions _options;
    private readonly IHeleketSigner _signer;

    public HeleketHttpSender(IFlurlClientCache clientCache, HeleketOptions options, IHeleketSigner signer)
    {
        _clientCache = clientCache;
        _options = options;
        _signer = signer;
    }

    public async Task<HeleketResponse<T>> PostPaymentAsync<T>(
        string path,
        object request,
        CancellationToken cancellationToken = default,
        string? cursor = null)
    {
        return await PostAsync<T>(path, request, _options.PaymentApiKey!, cancellationToken, cursor).ConfigureAwait(false);
    }

    private async Task<HeleketResponse<T>> PostAsync<T>(
        string path,
        object request,
        string apiKey,
        CancellationToken cancellationToken,
        string? cursor = null)
    {
        var json = HeleketJson.Serialize(request);

        try
        {
            var client = _clientCache.GetOrAdd(
                $"heleket:{_options.BaseUrl}",
                _options.BaseUrl,
                builder => builder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(_options.BaseUrl)));

            var httpRequest = client.Request(path);
            if (!string.IsNullOrWhiteSpace(cursor))
            {
                httpRequest = httpRequest.SetQueryParam("cursor", cursor);
            }

            var response = await httpRequest
                .WithHeaders(new
                {
                    merchant = _options.MerchantId,
                    sign = _signer.SignJson(json, apiKey)
                })
                .WithSettings(settings => settings.JsonSerializer = HeleketJson.FlurlSerializer)
                .PostJsonAsync(request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            var responseBody = await response.GetStringAsync().ConfigureAwait(false);
            var envelope = HeleketJson.Deserialize<HeleketResponse<T>>(responseBody);

            return envelope ?? throw new HeleketApiException(response.ResponseMessage.StatusCode, responseBody);
        }
        catch (FlurlHttpException ex)
        {
            var responseBody = await ex.GetResponseStringAsync().ConfigureAwait(false) ?? string.Empty;
            var statusCode = ex.Call.Response?.ResponseMessage.StatusCode ?? HttpStatusCode.InternalServerError;
            var envelope = HeleketJson.Deserialize<HeleketResponse<T>>(responseBody);
            if (envelope is not null)
            {
                return envelope;
            }

            throw new HeleketApiException(statusCode, responseBody);
        }
    }
}
