using System.Management.Automation;
using Perry.Options;

namespace Perry.Errors.Handlers
{
    internal interface IErrorHandler
    {
        void Add(PSCmdlet source, PerryOptions options);
        void Remove();
    }
}