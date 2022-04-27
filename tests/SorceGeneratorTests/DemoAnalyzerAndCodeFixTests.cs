using System.Threading.Tasks;
using SourceGenerator;
using Xunit;
using Verifier = SorceGeneratorTests.AnalyzerAndCodeFixVerifier<
    SourceGenerator.DemoAnalyzer,
    SourceGenerator.DemoCodeFixProvider>;

namespace SorceGeneratorTests;

public class DemoAnalyzerAndCodeFixTests
{
    [Fact]
    public async Task Should_trigger_on_non_partial_class()
    {
        var input = @"
using SourceGeneratorLibrary;

namespace DemoTests
{
   [EnumGeneration]
   public class {|#0:ProductCategory|}
   {
   }
}";

        var expectedOutput = @"
using SourceGeneratorLibrary;

namespace DemoTests
{
   [EnumGeneration]
   public partial class ProductCategory
   {
   }
}";

        var expectedError = Verifier
            .Diagnostic(DemoDiagnosticsDescriptors.EnumerationMustBePartial.Id)
            .WithLocation(0)
            .WithArguments("ProductCategory");

        await Verifier.VerifyCodeFixAsync(input, expectedOutput, expectedError);
    }
}