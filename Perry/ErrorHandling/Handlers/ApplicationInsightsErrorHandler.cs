using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Perry.Commands.Options;

namespace Perry.ErrorHandling.Handlers
{
    /// <summary>
    /// Log errors into Application Insights.
    /// </summary>
    class ApplicationInsightsErrorHandler : IErrorHandler, IDisposable
    {
        public static ApplicationInsightsErrorHandler Singleton => _singleton.Value;

        private PerryOptions _options;

        private static bool _isInitialized;
        private static readonly object _syncInitialize = new object();
        private static readonly Lazy<ApplicationInsightsErrorHandler> _singleton = new Lazy<ApplicationInsightsErrorHandler>(() => new ApplicationInsightsErrorHandler());

        private static TelemetryClient _telemetryClient;
        private bool _disposedValue;

        public ApplicationInsightsErrorHandler()
        {
            if (_isInitialized) return;

            lock (_syncInitialize)
            {
                if (_isInitialized) return;
                ErrorCatcher.Singleton.ErrorEvent += ErrorHandler;
                AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
                _isInitialized = true;
            }
        }

        public void Add(PSCmdlet source, PerryOptions options)
        {
            _options = options;
            var configuration = TelemetryConfiguration.CreateDefault();
            if (!configuration.TelemetryInitializers.Any(t => t is HttpDependenciesParsingTelemetryInitializer))
                configuration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());
            if (!configuration.TelemetryInitializers.Any(t => t is OperationCorrelationTelemetryInitializer))
                configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            configuration.ConnectionString = options.ApplicationInsightsConnectionString;

            var channel = new ServerTelemetryChannel();
            var telemetryFolder = Path.Combine(Path.GetTempPath(), "68d24d18-294f-46a4-bcea-e7feb374b516");
            Directory.CreateDirectory(telemetryFolder);
            channel.StorageFolder = telemetryFolder;
            configuration.TelemetryChannel = channel;
            channel.Initialize(configuration);

            _telemetryClient = new TelemetryClient(configuration);

            if (options.CustomProperties != null && options.CustomProperties.TryGetValue("CloudRoleName", out var cloudRoleName))
            {
                options.CustomProperties.Remove("CloudRoleName");
                _telemetryClient.Context.Cloud.RoleName = cloudRoleName;
            }

            if (options.CustomProperties != null && options.CustomProperties.TryGetValue("CloudRoleInstance", out var cloudRoleInstance))
            {
                options.CustomProperties.Remove("CloudRoleInstance");
                _telemetryClient.Context.Cloud.RoleInstance = cloudRoleInstance;
            }

            if (options.CustomProperties != null)
            {
                foreach (var property in options.CustomProperties)
                {
                    _telemetryClient.Context.GlobalProperties.Add(property.Key, property.Value);
                }
            }
        }

        private void OnDomainUnload(object sender, EventArgs e)
        {
            Dispose();
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
            client.FlushAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                var client = _telemetryClient;
                client?.FlushAsync(CancellationToken.None).GetAwaiter().GetResult();

                if (disposing)
                {
                }

                _telemetryClient = null;
                _disposedValue = true;
            }
        }

        ~ApplicationInsightsErrorHandler()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}