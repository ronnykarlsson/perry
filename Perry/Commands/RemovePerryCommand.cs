using System.Management.Automation;
using Perry.ErrorHandling.Handlers;

namespace Perry.Commands
{
    [Cmdlet(VerbsCommon.Remove, "Perry")]
    [OutputType(typeof(void))]
    public class RemovePerryCommand : PSCmdlet
    {
        protected override void EndProcessing()
        {
            InteractiveErrorHandler.Singleton.Remove();
            MemoryErrorHandler.Singleton.Remove();
            FileLoggingErrorHandler.Singleton.Remove();
            ApplicationInsightsErrorHandler.Singleton.Remove();
        }
    }
}