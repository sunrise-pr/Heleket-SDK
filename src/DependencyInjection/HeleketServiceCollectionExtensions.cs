using Flurl.Http.Configuration;
using Heleket.Abstractions;
using Heleket.Client;
using Heleket.Configuration;
using Heleket.Signing;
using Heleket.Webhooks.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heleket.DependencyInjection
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
                 .Bind(configuration.GetSection(configurationSectionName))
                 .Validate(ValidateOptions, "Heleket options are invalid.");

            AddCoreServices(services);

            return services;
        }

        private static void AddCoreServices(IServiceCollection services)
        {
            services.AddScoped<IHeleketApiClient, HeleketApiClient>();
            services.AddSingleton<IFlurlClientCache, FlurlClientCache>();
            services.AddSingleton<IHeleketSigner, HeleketSigner>();
            services.AddSingleton<IHeleketWebhookValidator, HeleketWebhookValidator>();
            services.AddScoped<IHeleketWebhookVerifier, HeleketWebhookVerifier>();
            services.AddScoped<IHeleketClient>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<HeleketOptions>>().Value;
                var clientCache = serviceProvider.GetRequiredService<IFlurlClientCache>();
                var signer = serviceProvider.GetRequiredService<IHeleketSigner>();
                var validator = serviceProvider.GetRequiredService<IHeleketWebhookValidator>();
                return new HeleketClient(options, clientCache, signer, validator);
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
