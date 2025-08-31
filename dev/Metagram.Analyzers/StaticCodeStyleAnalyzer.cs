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

    private static readonly DiagnosticDescriptor EmptyCatchRule = new DiagnosticDescriptor(
        id: "RCSR003", // Rikitav's code style Rule 003
        title: "Dont leave empty catch blocks",
        messageFormat: "Dont leave empty catch blocks — add exception handling or at least comment",
        category: "CodeStyle",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor BlockNamespaceRule = new DiagnosticDescriptor(
        id: "TMCSR001", // TheMakarik's code style Rule 001
        title: "Forbidden to use namespace declaration without semicolon'",
        messageFormat: "Dont use namespace declaration without semicolon like \"namespace {...}\", use like \"namespace;\"",
        category: "CodeStyle",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [VarRule, NewRule, BlockNamespaceRule, EmptyCatchRule];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(AnalyzeVar, SyntaxKind.VariableDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeNamespace, SyntaxKind.NamespaceDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeNew, SyntaxKind.ObjectCreationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeEmptyCatch, SyntaxKind.CatchClause);
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

    private void AnalyzeEmptyCatch(SyntaxNodeAnalysisContext context)
    {
        CatchClauseSyntax catchClause = (CatchClauseSyntax)context.Node;

        // Если тело catch пустое (нет операторов)
        if (!catchClause.Block.Statements.Any())
        {
            // Проверим, нет ли хотя бы комментариев
            IEnumerable<SyntaxTrivia> trivia = catchClause.Block.DescendantTrivia();
            bool hasComment = trivia.Any(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia) || t.IsKind(SyntaxKind.MultiLineCommentTrivia));

            if (!hasComment)
                context.ReportDiagnostic(Diagnostic.Create(EmptyCatchRule, catchClause.Block.GetLocation()));
        }
    }

    private void AnalyzeNamespace(SyntaxNodeAnalysisContext context)
    {
        NamespaceDeclarationSyntax namespaceDeclaration = (NamespaceDeclarationSyntax)context.Node;
        context.ReportDiagnostic(Diagnostic.Create(BlockNamespaceRule, namespaceDeclaration.Name.GetLocation()));
    }
}

