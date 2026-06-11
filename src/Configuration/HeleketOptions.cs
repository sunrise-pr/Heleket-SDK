using System.ComponentModel.DataAnnotations;

namespace Heleket.Configuration
{
    /// <summary>
    /// Configures Heleket SDK authentication and transport settings.
    /// </summary>
    public class HeleketOptions
    {
        /// <summary>
        /// Default configuration section name used by compatibility configuration binding.
        /// </summary>
        public const string DefaultSectionName = "Heleket";

        /// <summary>
        /// Default Heleket API base URL.
        /// </summary>
        public const string DefaultBaseUrl = "https://api.heleket.com/v1/";

        private string _baseUrl = DefaultBaseUrl;

        /// <summary>
        /// Merchant UUID sent in the `merchant` request header.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string MerchantId { get; set; } = null!;

        /// <summary>
        /// API key used for payment endpoints and payment webhook validation.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string? PaymentApiKey { get; set; }

        /// <summary>
        /// Optional API key used for payout endpoints and payout webhook validation.
        /// </summary>
        public string? PayoutApiKey { get; set; }

        /// <summary>
        /// Base URL for Heleket API endpoints.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [Url]
        public string BaseUrl
        {
            get => _baseUrl;
            set => _baseUrl = string.IsNullOrWhiteSpace(value) ? DefaultBaseUrl : value;
        }

        /// <summary>
        /// Compatibility alias for older SDK configuration.
        /// </summary>
        public string BaseApiUrl
        {
            get => BaseUrl;
            set => BaseUrl = value;
        }

        /// <summary>
        /// Compatibility alias for older SDK configuration.
        /// </summary>
        public string? ApiRequestKey
        {
            get => PaymentApiKey;
            set => PaymentApiKey = value;
        }

        /// <summary>
        /// Validates required SDK options.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when required settings are missing.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(MerchantId))
            {
                throw new InvalidOperationException($"{nameof(MerchantId)} is required.");
            }

            if (string.IsNullOrWhiteSpace(PaymentApiKey))
            {
                throw new InvalidOperationException($"{nameof(PaymentApiKey)} is required.");
            }

            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new InvalidOperationException($"{nameof(BaseUrl)} is required.");
            }
        }
    }
}
