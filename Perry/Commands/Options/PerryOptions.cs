using System.IO;

namespace Perry.Commands.Options
{
    class PerryOptions
    {
        public PerryOptions(bool interactive, string logPath, bool includeException, bool includeVariable, string instrumentationKey)
        {
            Interactive = interactive;
            LogPath = string.IsNullOrWhiteSpace(logPath) ? null : Path.GetFullPath(logPath);
            IncludeException = includeException;
            IncludeVariable = includeVariable;
            InstrumentationKey = instrumentationKey;
        }

        public bool Interactive { get; }
        public string LogPath { get; }
        public bool IncludeException { get; }
        public bool IncludeVariable { get; }
        public string InstrumentationKey { get; }
        public int MemoryErrorCount { get; } = 20;
    }
}