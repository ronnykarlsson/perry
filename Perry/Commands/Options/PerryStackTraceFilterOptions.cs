using System;
using System.Collections.Generic;

namespace Perry.Commands.Options
{
    static class PerryStackTraceFilterOptions
    {
        public static Dictionary<string, string> StackTraceRegex { get; } = new Dictionary<string, string> {{Environment.NewLine + @"at.*?(Pester\.psm1)[^\r\n]+", ""}};
    }
}