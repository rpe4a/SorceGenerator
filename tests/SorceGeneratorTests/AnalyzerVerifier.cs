using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceGeneratorLibrary;

namespace SorceGeneratorTests;

public static class AnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    public static DiagnosticResult Diagnostic(string diagnosticId)
    {
        return CSharpAnalyzerVerifier<TAnalyzer, XUnitVerifier>.Diagnostic(diagnosticId);
    }

    public static async Task VerifyAnalyzerAsync(
        string source,
        params DiagnosticResult[] expected)
    {
        var test = new AnalyzerTest(source, expected);
        await test.RunAsync(CancellationToken.None);
    }

    private class AnalyzerTest : CSharpAnalyzerTest<TAnalyzer, XUnitVerifier>
    {
        public AnalyzerTest(
            string source,
            params DiagnosticResult[] expected)
        {
            TestCode = source;
            ExpectedDiagnostics.AddRange(expected);
#if NET6_0
            ReferenceAssemblies = new ReferenceAssemblies(
                "net6.0",
                new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"),
                Path.Combine("ref", "net6.0"));
#else
         ReferenceAssemblies = ReferenceAssemblies.Net.Net50;
#endif

            TestState.AdditionalReferences.Add(typeof(EnumGenerationAttribute).Assembly);
        }
    }
}