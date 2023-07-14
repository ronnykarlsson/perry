using System.Collections.Generic;
using System.IO;

namespace Perry.Commands.Options
{
    class PerryOptions
    {
        public PerryOptions(bool interactive, string logPath, bool includeException, bool includeVariable, string applicationInsightsConnectionString, Dictionary<string, string> customProperties)
        {
            CustomProperties = customProperties;
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
        public Dictionary<string, string> CustomProperties { get; }
        public int MemoryErrorCount { get; } = 20;
    }
}