using System.Management.Automation;
using Perry.Commands.Options;

namespace Perry.Commands
{
    [Cmdlet(VerbsCommon.Add, "PerryStackTraceFilter")]
    [OutputType(typeof(void))]
    public class AddPerryStackTraceFilterCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Pattern { get; set; }

        [Parameter(Mandatory = true)]
        public string Replace { get; set; }

        protected override void EndProcessing()
        {
            PerryStackTraceFilterOptions.StackTraceRegex[Pattern] = Replace;
        }
    }
}