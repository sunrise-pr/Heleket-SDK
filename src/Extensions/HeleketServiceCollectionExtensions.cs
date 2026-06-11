using Heleket.Options;
using Heleket.Payments;
using Heleket.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options; // Required for AddHttpClient configuration and base options extensions
using System;
using System.Net.Http.Headers;
namespace Heleket.Extensions
{
    /// <summary>
    /// Provides dependency injection registration helpers for the Heleket SDK.
    /// </summary>
    public static class HeleketServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Heleket SDK services using direct options configuration.
        /// </summary>
        /// <param name="services">The service collection to register with.</param>
        /// <param name="configureOptions">The options configuration delegate.</param>
        /// <returns>The same service collection for chaining.</returns>
        public static IServiceCollection AddHeleket(
            this IServiceCollection services,
            Action<HeleketOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(configureOptions);

            services.AddOptions<HeleketOptions>()
                .Configure(configureOptions)
                .Validate(ValidateOptions, "Heleket options are invalid.");

            AddCoreServices(services);

            return services;
        }

        /// <summary>
        /// Adds Heleket SDK services (API Client, Webhook Verifier) and configuration to the DI container.
        /// </summary>
        /// <param name="services">The IServiceCollection.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="configurationSectionName">The name of the configuration section for Heleket settings (default: "Heleket").</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddHeleketSdk(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName = HeleketOptions.DefaultSectionName)
        {
            // 1. Configure and Validate Options
            // Binds configuration section to HeleketOptions and adds it to DI
            services.AddOptions<HeleketOptions>()
                 .Bind(configuration.GetSection(configurationSectionName)) // Теперь должно работать
                 //.ValidateDataAnnotations() // Использует атрибуты валидации в HeleketOptions
                 .Validate(ValidateOptions, "Heleket options are invalid.");

            AddCoreServices(services);

            // 2. Register HttpClient for the API Client...
            //services.AddHttpClient<IHeleketApiClient, HeleketApiClient>((serviceProvider, client) =>
            //{
            //    // ... (конфигурация HttpClient как раньше) ...
            //    var heleketOptions = serviceProvider.GetRequiredService<IOptions<HeleketOptions>>().Value;
            //    client.BaseAddress = new Uri(heleketOptions.BaseApiUrl);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    // ... (добавление заголовков авторизации, если нужно) ...
            //});

            // Add policies like Polly for resilience (timeouts, retries) if needed:
            // .AddPolicyHandler(...)

            // You could also register the Options class itself if needed directly elsewhere
            // services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<HeleketOptions>>().Value);

            return services;
        }

        private static void AddCoreServices(IServiceCollection services)
        {
            services.AddScoped<IHeleketApiClient, HeleketApiClient>();
            services.AddSingleton<IHeleketSigner, HeleketSigner>();
            services.AddSingleton<IHeleketWebhookValidator, HeleketWebhookValidator>();
            services.AddScoped<IHeleketWebhookVerifier, HeleketWebhookVerifier>();
            services.AddScoped<IHeleketClient>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<HeleketOptions>>().Value;
                var signer = serviceProvider.GetRequiredService<IHeleketSigner>();
                var validator = serviceProvider.GetRequiredService<IHeleketWebhookValidator>();
                return new HeleketClient(options, new HttpClient(), signer, validator);
            });
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IHeleketClient>().Payments);
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IHeleketClient>().Webhooks);
        }

        private static bool ValidateOptions(HeleketOptions options)
        {
            try
            {
                options.Validate();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
