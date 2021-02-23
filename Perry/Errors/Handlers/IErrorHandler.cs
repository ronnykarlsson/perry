using System.Management.Automation;

namespace Perry.Errors.Handlers
{
    internal interface IErrorHandler
    {
        void Add(PSCmdlet source, PerryOptions options);
        void Remove();
    }
}