using Xunit;
using VerifyCS = Funcky.Analyzers.Test.CSharpCodeFixVerifier<Funcky.Analyzers.UseWithArgumentNamesAnalyzer, Funcky.Analyzers.AddArgumentNameCodeFix>;

namespace Funcky.Analyzers.Test
{
    public sealed class UseWithArgumentNamesTest
    {
        [Fact]
        public async Task UsagesOfMethodsAnnotatedWithShouldUseNamedArgumentsAttributeGetWarning()
        {
            var expectedDiagnostics = new[]
            {
                VerifyCS.Diagnostic().WithSpan(11, 27, 11, 29).WithArguments("y"),
                VerifyCS.Diagnostic().WithSpan(12, 20, 12, 22).WithArguments("x"),
                VerifyCS.Diagnostic().WithSpan(12, 24, 12, 26).WithArguments("y"),
                VerifyCS.Diagnostic().WithSpan(14, 17, 14, 19).WithArguments("x"),
                VerifyCS.Diagnostic().WithSpan(14, 21, 14, 23).WithArguments("y"),
                VerifyCS.Diagnostic().WithSpan(16, 17, 16, 19).WithArguments("x"),
                VerifyCS.Diagnostic().WithSpan(17, 17, 17, 19).WithArguments("y"),
            };

            await VerifyWithSourceExample.VerifyDiagnosticAndCodeFix<UseWithArgumentNamesAnalyzer, AddArgumentNameCodeFix>(expectedDiagnostics, "UseWithArgumentNames");
        }
    }
}
