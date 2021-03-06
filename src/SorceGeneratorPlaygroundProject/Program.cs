using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Console;

namespace SorceGeneratorPlaygroundProject;

partial class Program
{
    const string programText =
        @"using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";

    const string programTextForSyntaxWalker =
        @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TopLevel
{
    using Microsoft;
    using System.ComponentModel;

    namespace Child1
    {
        using Microsoft.Win32;
        using System.Runtime.InteropServices;

        class Foo { }
    }

    namespace Child2
    {
        using System.CodeDom;
        using Microsoft.CSharp;

        class Bar { }
    }
}";

    static void Main(string[] args)
    {
        //SyntaxTreeManualTraversal();
        //SyntaxWalk();
        SemanticQuickStart();
    }

    private static void SemanticQuickStart()
    {

        SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);

        CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

        var compilation = CSharpCompilation.Create("HelloWorld")
            .AddReferences(MetadataReference.CreateFromFile(
                typeof(string).Assembly.Location))
            .AddSyntaxTrees(tree);

        var model = compilation.GetSemanticModel(tree);

        // Use the syntax tree to find "using System;"
        UsingDirectiveSyntax usingSystem = root.Usings[0];
        NameSyntax systemName = usingSystem.Name;

        // Use the semantic model for symbol information:
        SymbolInfo nameInfo = model.GetSymbolInfo(systemName);

        var systemSymbol = (INamespaceSymbol)nameInfo.Symbol;
        foreach (INamespaceSymbol ns in systemSymbol.GetNamespaceMembers())
        {
            Console.WriteLine(ns);
        }

        // Use the syntax model to find the literal string:
        LiteralExpressionSyntax helloWorldString = root.DescendantNodes()
            .OfType<LiteralExpressionSyntax>()
            .Single();

        // Use the semantic model for type information:
        TypeInfo literalInfo = model.GetTypeInfo(helloWorldString);

        var stringTypeSymbol = (INamedTypeSymbol)literalInfo.Type;

        var allMembers = stringTypeSymbol.GetMembers();
        var methods = allMembers.OfType<IMethodSymbol>();
        var publicStringReturningMethods = methods
            .Where(m => m.ReturnType.Equals(stringTypeSymbol) &&
                        m.DeclaredAccessibility == Accessibility.Public);
        var distinctMethods = publicStringReturningMethods.Select(m => m.Name).Distinct();
        foreach (string name in distinctMethods)
        {
            Console.WriteLine(name);
        }
    }

    private static void SyntaxWalk()
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(programTextForSyntaxWalker);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

        var collector = new UsingCollector();

        collector.Visit(root);

        foreach (var directive in collector.Usings)
        {
            WriteLine(directive.Name);
        }
    }

    private static void SyntaxTreeManualTraversal()
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(programText);
        CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

        WriteLine($"The tree is a {root.Kind()} node.");

        WriteLine($"The tree has {root.Members.Count} elements in it. They are:");
        WriteLine($"The tree has {root.Usings.Count} using statements. They are:");
        foreach (UsingDirectiveSyntax element in root.Usings)
            WriteLine($"\t{element.Name}");

        MemberDeclarationSyntax firstMember = root.Members[0];
        WriteLine($"The first member is a {firstMember.Kind()}.");

        var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;
        WriteLine($"There are {helloWorldDeclaration.Members.Count} members declared in this namespace.");
        WriteLine($"The first member is a {helloWorldDeclaration.Members[0].Kind()}.");

        var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
        WriteLine($"There are {programDeclaration.Members.Count} members declared in the {programDeclaration.Identifier} class.");
        WriteLine($"The first member is a {programDeclaration.Members[0].Kind()}.");

        var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];
        WriteLine($"The return type of the {mainDeclaration.Identifier} method is {mainDeclaration.ReturnType}.");
        WriteLine($"The method has {mainDeclaration.ParameterList.Parameters.Count} parameters.");
        foreach (ParameterSyntax item in mainDeclaration.ParameterList.Parameters)
            WriteLine($"The type of the {item.Identifier} parameter is {item.Type}.");
        WriteLine($"The body text of the {mainDeclaration.Identifier} method follows:");
        WriteLine(mainDeclaration.Body?.ToFullString());

        var argsParameter = mainDeclaration.ParameterList.Parameters[0];
        
        //Query methods
        var argsParameter2 = root
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .First(x => x.Identifier.ValueText == "Main")
            .ParameterList.Parameters.Single();
        
        Write(argsParameter == argsParameter2);


    }
}