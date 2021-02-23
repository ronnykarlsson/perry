using System.Management.Automation;
using Perry.Errors;
using Perry.Errors.Handlers;

namespace Perry
{
    [Cmdlet(VerbsCommon.Get, "Perry")]
    [OutputType(typeof(ErrorInformation[]))]
    public class GetPerryCommand : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var errors = MemoryErrorHandler.Singleton.GetErrors();
            foreach (var errorInformation in errors)
            {
                WriteObject(errorInformation);
            }
        }
    }
}