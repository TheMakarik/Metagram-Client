using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Metagram.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StaticCodeStyleAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor VarRule = new DiagnosticDescriptor(
            id: "RCSR001", // Rikitav's code style Rule 001
            title: "Запрещено использовать 'var'",
            messageFormat: "Не используйте 'var', укажите тип явно",
            category: "CodeStyle",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor NewRule = new DiagnosticDescriptor(
            id: "RCSR002", // Rikitav's code style Rule 002
            title: "Запрещено использовать неявную инициализацию 'new()'",
            messageFormat: "Не используйте 'new()', укажите тип явно",
            category: "CodeStyle",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [VarRule, NewRule];

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

            context.RegisterSyntaxNodeAction(AnalyzeVar, SyntaxKind.VariableDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeNew, SyntaxKind.ObjectCreationExpression);
        }
        private void AnalyzeVar(SyntaxNodeAnalysisContext context)
        {
            VariableDeclarationSyntax declaration = (VariableDeclarationSyntax)context.Node;
            if (declaration.Type.IsVar)
            {
                context.ReportDiagnostic(Diagnostic.Create(VarRule, declaration.Type.GetLocation()));
            }
        }

        private void AnalyzeNew(SyntaxNodeAnalysisContext context)
        {
            ObjectCreationExpressionSyntax creation = (ObjectCreationExpressionSyntax)context.Node;
            if (creation.Type.IsMissing)
            {
                context.ReportDiagnostic(Diagnostic.Create(NewRule, creation.GetLocation()));
            }
        }
    }
}
