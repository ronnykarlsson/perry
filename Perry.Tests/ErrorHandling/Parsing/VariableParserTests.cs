using System.Linq;
using Perry.ErrorHandling.Parsing;
using Xunit;

namespace Perry.Tests.ErrorHandling.Parsing
{
    public class VariableParserTests
    {
        public class GetVariablesMethod
        {
            [Fact]
            public void GetVariableWithNamespace()
            {
                var inputData = "10 / $local:myvariable";

                var result = VariableParser.GetVariables(inputData).Single();

                Assert.Equal("local:myvariable", result);
            }

            [Fact]
            public void GetVariableWithoutNamespace()
            {
                var inputData = "10 / $myvariable";

                var result = VariableParser.GetVariables(inputData).Single();

                Assert.Equal("myvariable", result);
            }

            [Fact]
            public void GetMultipleVariables()
            {
                var inputData = "$var1 / $var2";

                var result = VariableParser.GetVariables(inputData);

                Assert.Equal(new[] { "var1", "var2" }, result);
            }

            [Fact]
            public void GetNoVariable()
            {
                var inputData = "Cause-Error -Input 'Oops'";

                var result = VariableParser.GetVariables(inputData);

                Assert.Empty(result);
            }
        }
    }
}
