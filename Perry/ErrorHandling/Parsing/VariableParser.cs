using System.Management.Automation;
using System.Text.RegularExpressions;

namespace Perry.ErrorHandling.Parsing
{
    /// <summary>
    /// Parse variable names from script error.
    /// </summary>
    static class VariableParser
    {
        public static string[] GetVariables(string input)
        {
            var variablePattern = @"(?<=\$)([\w]+:)?(\w+)";

            var variables = Regex.Matches(input, variablePattern);

            var result = new string[variables.Count];
            for (int i = 0; i < variables.Count; i++)
            {
                var variableName = variables[i].Value;
                result[i] = variableName;
            }

            return result;
        }
    }
}
