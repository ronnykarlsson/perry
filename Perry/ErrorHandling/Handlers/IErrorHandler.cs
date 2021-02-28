using System.Management.Automation;
using Perry.Commands.Options;

namespace Perry.ErrorHandling.Handlers
{
    internal interface IErrorHandler
    {
        void Add(PSCmdlet source, PerryOptions options);
        void Remove();
    }
}