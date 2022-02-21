using System.Collections.Immutable;
using Funcky.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Funcky.SourceGenerator.TemplateLoader;

namespace Funcky.SourceGenerator
{
    [Generator(LanguageNames.CSharp)]
    public sealed class OrNoneFromTryPatternGenerator : IIncrementalGenerator
    {
        private const string OrNoneFromTryPatternAttributeTemplate = "OrNoneFromTryPatternAttribute";

        private static readonly IEnumerable<string> GeneratedFileHeadersSource = ImmutableList.Create("// <auto-generated/>", "#nullable enable", string.Empty);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(RegisterOrNoneAttribute);

            context.RegisterSourceOutput(GetOrNonePartialMethods(context), RegisterOrNonePartials);
        }

        private static void RegisterOrNonePartials(SourceProductionContext context, ImmutableArray<MethodPartial> partialMethods)
            => _ = partialMethods
                .GroupBy(partialMethod => $"{partialMethod.NamespaceName}.{partialMethod.ClassName}")
                .Aggregate(context, CreateSourceByClass);

        private static SourceProductionContext CreateSourceByClass(SourceProductionContext context, IGrouping<string, MethodPartial> methodByClass)
        {
            var usings = methodByClass.SelectMany(c => c.AdditionalUsings).Distinct(new UsingsComparer());
            var syntaxTree = OrNoneFromTryPatternPartial.GetSyntaxTree(methodByClass.First().NamespaceName, methodByClass.First().ClassName, usings, methodByClass.Select(m => m.PartialMethod));

            context.AddSource($"{methodByClass.First().ClassName}.g.cs", string.Join(Environment.NewLine, GeneratedFileHeadersSource) + Environment.NewLine + EmitCode(syntaxTree.NormalizeWhitespace()));

            return context;
        }

        private static IncrementalValueProvider<ImmutableArray<MethodPartial>> GetOrNonePartialMethods(IncrementalGeneratorInitializationContext context)
            => context.SyntaxProvider.CreateSyntaxProvider(predicate: IsSyntaxTargetForGeneration, transform: GetSemanticTargetForGeneration)
                .WhereNotNull()
                .Combine(context.CompilationProvider)
                .Select((state, cancellationToken) => ToMethodPartial(state.Left, state.Right, context, cancellationToken))
                .WhereNotNull()
                .Collect();

        private static MethodPartial? ToMethodPartial(MethodDeclarationSyntax methodDeclaration, Compilation compilation, IncrementalGeneratorInitializationContext context, CancellationToken cancellationToken)
        {
            var attribute = methodDeclaration.GetAttributeByUsedName("OrNoneFromTryPattern");
            var compilationUnit = methodDeclaration.TryGetParentSyntax<CompilationUnitSyntax>()!;

            return GetNamespaceName(methodDeclaration) is { } namespaceName && GetClassName(methodDeclaration) is { } className
                ? new MethodPartial(namespaceName, className, compilationUnit.Usings.ToList(), CreateMethodImplementation(methodDeclaration, GetMethodValue(compilation, methodDeclaration, attribute), GetTypeValue(compilation, methodDeclaration, attribute)))
                : null;
        }

        private static string GetMethodValue(Compilation compilation, MethodDeclarationSyntax method, AttributeSyntax attribute)
            => attribute.ArgumentList?.Arguments.Count > 1 && attribute.ArgumentList?.Arguments[1].Expression is { } descriptionExpr
                ? compilation
                    .GetSemanticModel(method.SyntaxTree)
                    .GetConstantValue(descriptionExpr)
                    .ToString()
                : throw new Exception("Method value on attribute missing.");

        private static TypeSyntax GetTypeValue(Compilation compilation, MethodDeclarationSyntax component, AttributeSyntax attribute)
            => attribute.ArgumentList?.Arguments.Count > 0 && attribute.ArgumentList?.Arguments[0].Expression is TypeOfExpressionSyntax iconExpr
                ? iconExpr.Type
                : throw new Exception("Type value on attribute missing.");

        private static string? GetClassName(SyntaxNode methodDeclaration)
            => methodDeclaration
                .TryGetParentSyntax<ClassDeclarationSyntax>()
                ?.Identifier
                .ToString();

        private static string? GetNamespaceName(SyntaxNode methodDeclaration)
            => methodDeclaration
                .TryGetParentSyntax<NamespaceDeclarationSyntax>()
                ?.Name
                .ToString();

        private static MethodDeclarationSyntax CreateMethodImplementation(MethodDeclarationSyntax methodDeclaration, string methodName, TypeSyntax typeSyntax)
            => (MethodDeclarationSyntax)new OrNoneFromTryPatternRewriter(methodName, typeSyntax)
                .Visit(methodDeclaration);

        private static string EmitCode(SyntaxNode syntaxNode)
            => syntaxNode.ToFullString();

        private static MethodDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellationToken)
            => context.Node is MethodDeclarationSyntax methodDeclaration && methodDeclaration.AttributeLists.Any(HasOrNoneAttribute(context, cancellationToken))
                ? methodDeclaration
                : null;

        private static Func<AttributeListSyntax, bool> HasOrNoneAttribute(GeneratorSyntaxContext context, CancellationToken cancellationToken)
            => attributeList
                => attributeList.Attributes.Any(IsDiscriminatedUnionAttribute(context, cancellationToken));

        private static Func<AttributeSyntax, bool> IsDiscriminatedUnionAttribute(GeneratorSyntaxContext context, CancellationToken cancellationToken)
            => attribute
                => context.SemanticModel.GetSymbolInfo(attribute, cancellationToken).Symbol is IMethodSymbol attributeSymbol
                    && attributeSymbol.ContainingType.ToDisplayString() == "Funcky.Internal.OrNoneFromTryPatternAttribute";

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken cancellationToken)
            => node is MethodDeclarationSyntax { AttributeLists.Count: > 0 };

        private static void RegisterOrNoneAttribute(IncrementalGeneratorPostInitializationContext context)
            => context.AddSource("OrNoneFromTryPatternAttribute.g.cs", CodeFromTemplate(OrNoneFromTryPatternAttributeTemplate));
    }
}
