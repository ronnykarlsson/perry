using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Perry.IntegrationTests
{
    public class PowerShellTestsBase
    {
        private readonly ITestOutputHelper _output;

        protected PowerShellTestsBase(ITestOutputHelper output)
        {
            _output = output;
        }

        protected void RunTests(string testFile)
        {
            string processArgument;
            bool useShellExecute;
            bool redirectOutput;
            int timeout;

            if (Debugger.IsAttached)
            {
                processArgument = $@"-NoProfile -NoExit -Command "".\RunTests.ps1 -Path '.\{testFile}'""";
                useShellExecute = true;
                redirectOutput = false;
                timeout = int.MaxValue;
            }
            else
            {
                processArgument = $@"-NoProfile -Command "".\RunTests.ps1 -Path '.\{testFile}'""";
                useShellExecute = false;
                redirectOutput = true;
                timeout = 10000;
            }

            var processInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = processArgument,
                UseShellExecute = useShellExecute,
                RedirectStandardOutput = redirectOutput,
                RedirectStandardError = redirectOutput
            };

            var process = new Process { StartInfo = processInfo };

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                    _output.WriteLine(args.Data);

            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    _output.WriteLine("Error: " + args.Data);
            };

            try
            {
                process.Start();
                if (redirectOutput)
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                }
                process.WaitForExit(timeout);
            }
            finally
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }

            Assert.NotNull(process);
            Assert.Equal(0, process.ExitCode);
        }
    }
}