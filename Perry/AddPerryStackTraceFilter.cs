using System.Management.Automation;
using Perry.Options;

namespace Perry
{
    [Cmdlet(VerbsCommon.Add, "PerryStackTraceFilter")]
    [OutputType(typeof(void))]
    public class AddPerryStackTraceFilter : PSCmdlet
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