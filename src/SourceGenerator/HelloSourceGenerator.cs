using Microsoft.CodeAnalysis;

namespace SourceGenerator
{
    [Generator]
    public class HelloSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);
            string source = $@" // Auto-generated code
                            using System;

                            namespace {mainMethod.ContainingNamespace.ToDisplayString()}
                            {{
                                public static partial class {mainMethod.ContainingType.Name}
                                {{
                                    static partial void HelloFrom(string name) =>
                                        Console.WriteLine($""Generator says: Hi from '{{name}}'"");
                                }}
                            }}
                            ";

            var typeName = mainMethod.ContainingType.Name;

            context.AddSource($"{typeName}.g.cs", source);
        }
    }
}