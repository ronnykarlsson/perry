using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Perry.Commands.Options;

namespace Perry.ErrorHandling.Handlers
{
    /// <summary>
    /// Log errors into memory to have them retrievable on-demand with GetPerryCommand.
    /// </summary>
    class MemoryErrorHandler : IErrorHandler
    {
        public static MemoryErrorHandler Singleton => _singleton.Value;

        private PerryOptions _options;

        private static bool _isInitialized;
        private static readonly object _syncInitialize = new object();
        private static readonly Lazy<MemoryErrorHandler> _singleton = new Lazy<MemoryErrorHandler>(() => new MemoryErrorHandler());

        private readonly object _errorsSyncObject = new object();
        private List<ErrorInformation> _errors = new List<ErrorInformation>();

        public MemoryErrorHandler()
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
            _options = options;
        }

        public void Remove()
        {
            _options = null;
            lock (_errorsSyncObject)
            {
                _errors.Clear();
            }
        }

        public ErrorInformation[] GetErrors()
        {
            if (_options == null) throw new InvalidOperationException("Run Add-Perry first");

            lock (_errorsSyncObject)
            {
                var errorCount = _options.MemoryErrorCount;

                if (_errors.Count > errorCount)
                {
                    _errors = _errors.Skip(_errors.Count - errorCount).ToList();

                }

                return _errors.ToArray();
            }
        }

        private void ErrorHandler(object sender, ErrorInformation e)
        {
            if (_options == null) return;

            lock (_errorsSyncObject)
            {
                _errors.Add(e);
            }
        }
    }
}