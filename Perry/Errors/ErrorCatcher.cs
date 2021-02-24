using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using Perry.Options;

namespace Perry.Errors
{
    /// <summary>
    /// Catch first chance exceptions and send them to errors handlers.
    /// </summary>
    class ErrorCatcher
    {
        public event EventHandler<ErrorInformation> ErrorEvent;
        public static ErrorCatcher Singleton => _singleton.Value;

        private string _lastError;
        private DateTime _lastErrorTime;

        private static bool _isInitialized;
        private static readonly object _syncInitialize = new object();
        private static readonly Lazy<ErrorCatcher> _singleton = new Lazy<ErrorCatcher>(() => new ErrorCatcher());

        public ErrorCatcher()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_isInitialized) return;

            lock (_syncInitialize)
            {
                if (_isInitialized) return;
                AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;
                _isInitialized = true;
            }
        }

        private void OnError(ErrorInformation error)
        {
            var handler = ErrorEvent;
            handler?.Invoke(this, error);
        }

        private void OnFirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            try
            {
                var runtimeException = e.Exception as RuntimeException;
                if (runtimeException?.ErrorRecord?.InvocationInfo == null) return;

                var errorRecord = runtimeException.ErrorRecord;

                var stackTrace = errorRecord.ScriptStackTrace;
                foreach (var regexFilter in PerryStackTraceFilterOptions.StackTraceRegex)
                {
                    stackTrace = Regex.Replace(stackTrace, regexFilter.Key, regexFilter.Value);
                }

                var errorInfo = $"{runtimeException.Message}\n{errorRecord.InvocationInfo.PositionMessage}\n{stackTrace}";

                var currentTime = DateTime.UtcNow;

                // The same exception is thrown repeatedly. Workaround to avoid spam until a better way is figured out.
                if (_lastError == errorInfo && _lastErrorTime > currentTime.AddMilliseconds(-100)) return;

                _lastError = errorInfo;
                _lastErrorTime = currentTime;

                OnError(new ErrorInformation
                {
                    Error = errorInfo,
                    Exception = runtimeException,
                    ErrorRecord = errorRecord,
                    DateTime = currentTime
                });
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.ToString());
            }
        }
    }
}
