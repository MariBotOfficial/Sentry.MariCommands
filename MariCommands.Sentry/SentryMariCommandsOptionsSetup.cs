using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Sentry.Extensions.Logging;

namespace MariCommands.Sentry
{
    internal class SentryMariCommandsOptionsSetup : ConfigureFromConfigurationOptions<SentryMariCommandsOptions>
    {
        private readonly IHostEnvironment _hostingEnvironment;

        public SentryMariCommandsOptionsSetup(
            ILoggerProviderConfiguration<SentryMariCommandsLoggerProvider> providerConfiguration,
            IHostEnvironment hostingEnvironment)
            : base(providerConfiguration.Configuration)
            => _hostingEnvironment = hostingEnvironment;

        public override void Configure(SentryMariCommandsOptions options)
        {
            base.Configure(options);

            options.Environment
                = options.Environment // Don't override user defined value, from SentryAspNetCore
                  ?? EnvironmentLocator.Locate() // Sentry specific environment takes precedence #92, from SentryAspNetCore
                  ?? _hostingEnvironment?.EnvironmentName;
        }
    }
}