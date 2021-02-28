using System.Collections.Generic;

namespace Perry.Commands.Options
{
    static class PerryStackTraceFilterOptions
    {
        public static Dictionary<string, string> StackTraceRegex { get; } = new Dictionary<string, string> {{@"\nat[^$]+Modules\\Pester[^$]+", "\n"}};
    }
}