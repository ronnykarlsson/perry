using System;
using System.Management.Automation;

namespace Perry.ErrorHandling
{
    class ErrorInformation
    {
        public string ErrorMessage { get; set; }
        public RuntimeException Exception { get; set; }
        public ErrorRecord ErrorRecord { get; set; }
        public DateTime DateTime { get; set; }
    }
}