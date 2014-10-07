using System.Linq;
using Microsoft.VisualStudio.Text;

namespace VsEmacs
{
    internal static class ITextBufferExtensions
    {
        internal static ITextSnapshotLine GetContainingLine(this ITextBuffer textBuffer, int position)
        {
            return textBuffer.CurrentSnapshot.Lines.FirstOrDefault(l =>
            {
                if (l.Start <= position)
                    return (int) l.End >= position;
                return false;
            });
        }

        internal static int GetLineNumber(this ITextBuffer textBuffer, SnapshotPoint position)
        {
            return textBuffer.GetContainingLine(position.Position).LineNumber;
        }
    }
}