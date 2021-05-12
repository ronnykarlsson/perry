using System.Management.Automation;
using Perry.Commands.Options;
using Perry.ErrorHandling.Handlers;

namespace Perry.Commands
{
    [Cmdlet(VerbsCommon.Add, "Perry")]
    [OutputType(typeof(void))]
    public class AddPerryCommand : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public SwitchParameter Interactive { get; set; }

        [Parameter(Mandatory = false)]
        public string LogPath { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter IncludeException { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter IncludeVariable { get; set; }

        [Parameter(Mandatory = false)]
        public string InstrumentationKey { get; set; }

        protected override void EndProcessing()
        {
            var perryOptions = new PerryOptions(Interactive, LogPath, IncludeException, IncludeVariable, InstrumentationKey);

            MemoryErrorHandler.Singleton.Add(this, perryOptions);

            if (Interactive)
            {
                InteractiveErrorHandler.Singleton.Add(this, perryOptions);
            }

            if (!string.IsNullOrWhiteSpace(LogPath))
            {
                FileLoggingErrorHandler.Singleton.Add(this, perryOptions);
            }

            if (!string.IsNullOrWhiteSpace(InstrumentationKey))
            {
                ApplicationInsightsErrorHandler.Singleton.Add(this, perryOptions);
            }
        }
    }
}
