using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Funcky.SourceGenerator;

internal record MethodPartial(string NamespaceName, string ClassName, IEnumerable<UsingDirectiveSyntax> AdditionalUsings, MethodDeclarationSyntax PartialMethod, IReadOnlyCollection<Diagnostic>? Diagnostics = null);
