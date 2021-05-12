using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Perry.Commands.Options;

namespace Perry.ErrorHandling.Handlers
{
    /// <summary>
    /// Log errors into memory to have them retrievable on-demand with GetPerryCommand.
    /// </summary>
    class ApplicationInsightsErrorHandler : IErrorHandler
    {
        public static ApplicationInsightsErrorHandler Singleton => _singleton.Value;

        private PerryOptions _options;

        private static bool _isInitialized;
        private static readonly object _syncInitialize = new object();
        private static readonly Lazy<ApplicationInsightsErrorHandler> _singleton = new Lazy<ApplicationInsightsErrorHandler>(() => new ApplicationInsightsErrorHandler());

        private static TelemetryClient _telemetryClient;

        public ApplicationInsightsErrorHandler()
        {
            if (_isInitialized) return;

            lock (_syncInitialize)
            {
                if (_isInitialized) return;
                ErrorCatcher.Singleton.ErrorEvent += ErrorHandler;
                AppDomain.CurrentDomain.ProcessExit += OnAppdomainExit;
                _isInitialized = true;
            }
        }

        public void Add(PSCmdlet source, PerryOptions options)
        {
            _options = options;
            _telemetryClient = new TelemetryClient(new TelemetryConfiguration(options.InstrumentationKey));
        }

        private void OnAppdomainExit(object sender, EventArgs e)
        {
            var client = _telemetryClient;
            client?.Flush();
        }

        public void Remove()
        {
            _options = null;
            _telemetryClient = null;
        }

        private void ErrorHandler(object sender, ErrorInformation e)
        {
            if (_options == null) return;

            var client = _telemetryClient;
            if (client == null) return;

            var exception = e.Exception.InnerException ?? e.Exception;
            var properties = new Dictionary<string, string>
            {
                {"ErrorMessage", e.ErrorMessage}
            };

            if (e.ErrorRecord != null)
            {
                properties.Add("ScriptStackTrace", e.ErrorRecord.ScriptStackTrace);
            }

            if (e.ErrorRecord?.InvocationInfo != null) properties.Add("PSCommandPath", e.ErrorRecord.InvocationInfo.PSCommandPath);

            client.TrackException(exception, properties);
        }
    }
}