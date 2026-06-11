using Flurl; // Required for Url methods
using Flurl.Http; // Required for Flurl HTTP extension methods
using Flurl.Http.Configuration;
using Heleket.Internal;
using Heleket.Models;
using Heleket.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json;
namespace Heleket.Services
{
    /// <summary>
    /// Legacy Flurl-based Heleket API client retained for compatibility.
    /// </summary>
    public class HeleketApiClient : IHeleketApiClient
    {
        private readonly HeleketOptions _options;
        private readonly ILogger<HeleketApiClient>? _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Gets the configured payment API key compatibility alias.
        /// </summary>
        public string ApiKey
        {
            get
            {
                return _options.ApiRequestKey ?? string.Empty;

            }
        }

        /// <summary>
        /// Creates the legacy API client.
        /// </summary>
        /// <param name="options">The configured Heleket options.</param>
        /// <param name="logger">The logger instance.</param>
        public HeleketApiClient(
            IOptions<HeleketOptions> options,
            ILogger<HeleketApiClient> logger)
        {
            _options = options.Value;
            _logger = logger;

            _jsonSerializerOptions = HeleketJson.Options;

            if (string.IsNullOrWhiteSpace(_options.ApiRequestKey))
            {
                _logger?.LogWarning("Heleket ApiRequestKey is not configured. API calls might fail if required.");
            }

        }

        /// <inheritdoc />
        public async Task<(string? Status, decimal? PayerAmountUsd)> GetPaymentInfoAsync(string uuid, string orderId, CancellationToken cancellationToken = default)
        {
            string endpointPath = "info";

            try
            {
                var flurlRequest = _options.BaseApiUrl
                    .AppendPathSegment("payment").AppendPathSegment(endpointPath);

                var request = new Dictionary<string, string>
                {
                    { "uuid", uuid },
                    { "orderId", orderId }
                };

                var rt = await flurlRequest
                    .WithHeaders(new { ContentType = "application/json", merchant = _options.MerchantId, sign = SignatureGenerator.GenerateRequestSignature(request, _options.ApiRequestKey) })
                    .WithSettings(settings =>
                    {
                        settings.JsonSerializer = new DefaultJsonSerializer(_jsonSerializerOptions);
                    })
                    .PostJsonAsync(request, cancellationToken: cancellationToken);

                var response = await rt.GetJsonAsync<CreateInvoiceResponse>();

                _logger?.LogInformation("Payment info retrieved. OrderId={OrderId}, Status={Status}",
                    response?.Result?.OrderId, response?.Result?.PaymentStatus);

                decimal? payerAmount = null;
                if (decimal.TryParse(response?.Result?.PayerAmount,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out var parsed))
                {
                    payerAmount = parsed;
                }

                return (response?.Result?.PaymentStatus, payerAmount);
            }
            catch (FlurlHttpException flurlEx)
            {
                var statusCode = flurlEx.StatusCode;
                string errorBody = await flurlEx.GetResponseStringAsync() ?? "[No Response Body]";
                _logger?.LogError(flurlEx, "Heleket API request failed. Status Code: {StatusCode}, Body: {ErrorBody}",
                    statusCode, errorBody);
                return (null, null);
            }
            catch (TaskCanceledException cancelEx) when (cancelEx.CancellationToken == cancellationToken)
            {
                _logger?.LogWarning("Heleket API call was cancelled.");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An unexpected error occurred in HeleketApiClient.GetPaymentInfoAsync.");
                return (null, null);
            }
        }
        /// <inheritdoc />
        public async Task<CreateInvoiceResponse> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
        {
            string endpointPath = "payment"; // Adjust based on actual API endpoint

            try
            {
                // Construct the request using Flurl
                var flurlRequest = _options.BaseApiUrl // Start with the base URL from options
                    .AppendPathSegment(endpointPath); // Append the specific endpoint path
                                                      // ** Add Authentication Header if needed **
                                                      // .WithHeader("X-API-KEY", _options.ApiRequestKey); // Example if key needed per request

                var settings_j = new JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                    Formatting = Formatting.None
                };




                // Configure serialization and make the request
                var rt = await flurlRequest
                    .WithHeaders(new { ContentType = "application/json", merchant = _options.MerchantId, sign = SignatureGenerator.GenerateRequestSignature(request, _options.ApiRequestKey) })
                    .WithSettings(settings => // Configure serialization for this specific request
                    {
                        // Use the pre-configured options
                        settings.JsonSerializer = new DefaultJsonSerializer(_jsonSerializerOptions);

                    })
                    .PostJsonAsync(request, cancellationToken: cancellationToken);

                var response = await rt.GetJsonAsync<CreateInvoiceResponse>();// Send the request object as JSON body 

                _logger?.LogInformation("Successfully created/retrieved invoice with Order ID: {OrderId}", response?.Result?.OrderId);
                return response ?? new CreateInvoiceResponse { State = 1, Message = "Empty Heleket response." };
            }
            catch (FlurlHttpException flurlEx)
            {

                var statusCode = flurlEx.StatusCode;
                // Use TryGetResponseStringAsync for safety in case response is disposed or contentless
                string errorBody = await flurlEx.GetResponseStringAsync() ?? "[No Response Body]";

                _logger?.LogError(flurlEx, "Heleket API request failed. Status Code: {StatusCode}, Body: {ErrorBody}",
                    statusCode, errorBody);
                return new CreateInvoiceResponse { State = 1, Message = errorBody };
            }
            catch (TaskCanceledException cancelEx) when (cancelEx.CancellationToken == cancellationToken)
            {
                _logger?.LogWarning("Heleket API call was cancelled.");
                throw; // Re-throw cancellation
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An unexpected error occurred in HeleketApiClient.CreateInvoiceAsync.");
                return new CreateInvoiceResponse { State = 1, Message = ex.Message };
            }
        }

        // Implement other API client methods here using Flurl
    }
}
