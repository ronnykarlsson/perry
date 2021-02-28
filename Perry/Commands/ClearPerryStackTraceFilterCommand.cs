using System.Management.Automation;
using Perry.Commands.Options;

namespace Perry.Commands
{
    [Cmdlet(VerbsCommon.Clear, "PerryStackTraceFilter")]
    [OutputType(typeof(void))]
    public class ClearPerryStackTraceFilterCommand : PSCmdlet
    {
        protected override void EndProcessing()
        {
            PerryStackTraceFilterOptions.StackTraceRegex.Clear();
        }
    }
}