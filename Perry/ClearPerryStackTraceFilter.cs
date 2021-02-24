using System.Management.Automation;
using Perry.Options;

namespace Perry
{
    [Cmdlet(VerbsCommon.Clear, "PerryStackTraceFilter")]
    [OutputType(typeof(void))]
    public class ClearPerryStackTraceFilter : PSCmdlet
    {
        protected override void EndProcessing()
        {
            PerryStackTraceFilterOptions.StackTraceRegex.Clear();
        }
    }
}