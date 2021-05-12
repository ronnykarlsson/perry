using Xunit;
using Xunit.Abstractions;

namespace Perry.IntegrationTests
{
    public class GetPerryTests : PowerShellTestsBase
    {
        public GetPerryTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Run()
        {
            RunTests("GetPerryTests.ps1");
        }
    }
}
