using Microsoft.CodeAnalysis;

namespace RoslynAnalyzers
{
    /// <summary>
    /// Provides helper methods for working with source file locations.
    /// </summary>
    internal static class LocationHelpers
    {
        /// <summary>
        /// Gets the location in terms of path, line and column for a given node.
        /// </summary>
        /// <param name="node">The node to use.</param>
        /// <returns>The location in terms of path, line and column for a given node.</returns>
        internal static FileLinePositionSpan GetLineSpan(this SyntaxNode node) =>
            node.SyntaxTree.GetLineSpan(node.Span);

        /// <summary>
        /// Get a value indicating whether the given node span multiple source text lines.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <returns>True, if the node spans multiple source text lines.</returns>
        internal static bool SpansMultipleLines(this SyntaxNode node)
        {
            var lineSpan = node.GetLineSpan();

            return lineSpan.StartLinePosition.Line < lineSpan.EndLinePosition.Line;
        }
    }
}