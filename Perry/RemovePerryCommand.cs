using System.Management.Automation;
using Perry.Errors.Handlers;

namespace Perry
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
        }
    }
}