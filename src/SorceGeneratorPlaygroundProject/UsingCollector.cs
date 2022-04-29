using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Console;

namespace SorceGeneratorPlaygroundProject;

public class UsingCollector : CSharpSyntaxWalker
{
    public ICollection<UsingDirectiveSyntax> Usings { get; } = new List<UsingDirectiveSyntax>();

    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
        WriteLine($"\tVisitUsingDirective called with {node.Name}.");
        if (node.Name.ToString() != "System" &&
            !node.Name.ToString().StartsWith("System."))
        {
            WriteLine($"\t\tSuccess. Adding {node.Name}.");
            this.Usings.Add(node);
        }
    }
}