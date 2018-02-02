using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Text;

namespace RoslynAnalyzers
{
    /// <summary>
    /// Implements a code fix for <see cref="SA1413UseTrailingCommasInMultiLineInitializers" />.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SA1413CodeFixProvider))]
    [Shared]
    public class SA1413CodeFixProvider : CodeFixProvider
    {
        /// <inheritdoc />
        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(SA1413UseTrailingCommasInMultiLineInitializers.DiagnosticId);

        private static async Task<Document> GetTransformedDocumentAsync(
            Document document,
            Diagnostic diagnostic,
            CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
            var syntaxNode = syntaxRoot.FindNode(diagnostic.Location.SourceSpan);

            TextChange textChange = new TextChange(diagnostic.Location.SourceSpan, syntaxNode.ToString() + ",");
            return document.WithText(text.WithChanges(textChange));
        }

        /// <inheritdoc />
        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <inheritdoc />
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        Resources.SA1413CodeFix,
                        cancellationToken =>
                            GetTransformedDocumentAsync(context.Document, diagnostic, cancellationToken),
                        nameof(SA1413CodeFixProvider)),
                    diagnostic);
            }

            return Task.CompletedTask;
        }
    }
}