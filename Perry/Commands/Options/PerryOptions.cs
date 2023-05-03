using System.IO;

namespace Perry.Commands.Options
{
    class PerryOptions
    {
        public PerryOptions(bool interactive, string logPath, bool includeException, bool includeVariable, string applicationInsightsConnectionString)
        {
            Interactive = interactive;
            LogPath = string.IsNullOrWhiteSpace(logPath) ? null : Path.GetFullPath(logPath);
            IncludeException = includeException;
            IncludeVariable = includeVariable;
            ApplicationInsightsConnectionString = applicationInsightsConnectionString;
        }

        public bool Interactive { get; }
        public string LogPath { get; }
        public bool IncludeException { get; }
        public bool IncludeVariable { get; }
        public string ApplicationInsightsConnectionString { get; }
        public int MemoryErrorCount { get; } = 20;
    }
}