using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Funcky.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class UseWithArgumentNamesAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = $"{DiagnosticName.Prefix}{DiagnosticName.Usage}03";
    private const string Category = nameof(Funcky);

    private const string AttributeFullName = "Funcky.CodeAnalysis.UseWithArgumentNamesAttribute";

    private static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        id: DiagnosticId,
        title: Resources.UseWithArgumentNamesAnalyzerAnalyzerTitle,
        messageFormat: Resources.UseWithArgumentNamesAnalyzerMessageFormat,
        description: Resources.UseWithArgumentNamesAnalyzerDescription,
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterCompilationStartAction(static context =>
        {
            if (context.Compilation.GetTypeByMetadataName(AttributeFullName) is { } attributeSymbol)
            {
                context.RegisterOperationAction(AnalyzeInvocation(attributeSymbol), OperationKind.Invocation);
            }
        });
    }

    private static Action<OperationAnalysisContext> AnalyzeInvocation(INamedTypeSymbol attributeSymbol)
        => context =>
        {
            var invocation = (IInvocationOperation)context.Operation;

            if (invocation.TargetMethod.GetAttributes().Any(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeSymbol)))
            {
                foreach (var argument in invocation.Arguments)
                {
                    if (argument.Syntax is ArgumentSyntax { NameColon: null } argumentSyntax
                        && argument.Parameter?.Name is { } parameterName)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptor, argumentSyntax.GetLocation(), parameterName));
                    }
                }
            }
        };
}
