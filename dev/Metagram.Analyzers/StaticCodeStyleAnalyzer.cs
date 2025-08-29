namespace Metagram.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class StaticCodeStyleAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor VarRule = new DiagnosticDescriptor(
        id: "RCSR001", // Rikitav's code style Rule 001
        title: "Dont use 'var'",
        messageFormat: "Do not use 'var', set the type explicitly",
        category: "CodeStyle",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor NewRule = new DiagnosticDescriptor(
        id: "RCSR002", // Rikitav's code style Rule 002
        title: "Forbidden to use 'new()'",
        messageFormat: "Don't 'new()', set the type explicitly",
        category: "CodeStyle",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor BlockNamespaceRule = new DiagnosticDescriptor(
        id: "TMCSR001", // TheMakarik's code style Rule 001
        title: "Forbidden to use namespace declaration without semicolon'",
        messageFormat:
        "Dont use namespace declaration without semicolon like \"namespace {...}\", use like \"namespace;\"",
        category: "CodeStyle",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        [VarRule, NewRule, BlockNamespaceRule];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(AnalyzeVar, SyntaxKind.VariableDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeBlockNamespace, SyntaxKind.NamespaceDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeNew, SyntaxKind.ObjectCreationExpression);
    }

    private void AnalyzeVar(SyntaxNodeAnalysisContext context)
    {
        VariableDeclarationSyntax declaration = (VariableDeclarationSyntax)context.Node;
        if (declaration.Type.IsVar)
            context.ReportDiagnostic(Diagnostic.Create(VarRule, declaration.Type.GetLocation()));
    }

    private void AnalyzeNew(SyntaxNodeAnalysisContext context)
    {
        ObjectCreationExpressionSyntax creation = (ObjectCreationExpressionSyntax)context.Node;
        if (creation.Type.IsMissing)
            context.ReportDiagnostic(Diagnostic.Create(NewRule, creation.GetLocation()));
    }

    private void AnalyzeBlockNamespace(SyntaxNodeAnalysisContext context)
    {
        NamespaceDeclarationSyntax namespaceDeclaration = (NamespaceDeclarationSyntax)context.Node;
        context.ReportDiagnostic(Diagnostic.Create(BlockNamespaceRule, namespaceDeclaration.GetLocation()));
    }
}

