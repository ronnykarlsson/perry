using System;
using System.Management.Automation;
using Perry.Commands.Options;
using Perry.ErrorHandling.Parsing;

namespace Perry.ErrorHandling.Handlers
{
    /// <summary>
    /// Output errors as they happen.
    /// </summary>
    class InteractiveErrorHandler : IErrorHandler
    {
        public static InteractiveErrorHandler Singleton => _singleton.Value;

        private PSCmdlet _cmdlet;
        private PerryOptions _options;

        private static bool _isInitialized;
        private static readonly object _syncInitialize = new object();
        private static readonly Lazy<InteractiveErrorHandler> _singleton = new Lazy<InteractiveErrorHandler>(() => new InteractiveErrorHandler());

        public InteractiveErrorHandler()
        {
            if (_isInitialized) return;

            lock (_syncInitialize)
            {
                if (_isInitialized) return;
                ErrorCatcher.Singleton.ErrorEvent += ErrorHandler;
                _isInitialized = true;
            }
        }

        public void Add(PSCmdlet source, PerryOptions options)
        {
            _cmdlet = source;
            _options = options;
        }

        public void Remove()
        {
            _cmdlet = null;
        }

        public void ErrorHandler(object sender, ErrorInformation errorInformation)
        {
            var cmdlet = _cmdlet;
            if (cmdlet == null) return;

            var exceptionName = errorInformation.Exception.InnerException?.GetType().Name ?? errorInformation.Exception.GetType().Name;

            var errorMessage = $"Perry ({exceptionName}): {errorInformation.ErrorMessage}";

            if (_options?.IncludeVariable == true)
            {
                var variables = VariableParser.GetVariables(errorInformation.ErrorMessage);
                foreach (var variable in variables)
                {
                    var variableValue = cmdlet.GetVariableValue(variable).ToString();
                    errorMessage += $"{Environment.NewLine}   Variable: ${variable} = {variableValue}";
                }
            }

            if (_options?.IncludeException == true)
            {
                errorMessage += $"{Environment.NewLine}{errorInformation.Exception}";
            }

            cmdlet.WriteWarning(errorMessage);
        }
    }
}
