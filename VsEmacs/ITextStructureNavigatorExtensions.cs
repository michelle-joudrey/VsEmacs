using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;

namespace VsEmacs
{
    internal static class ITextStructureNavigatorExtensions
    {
        internal static SnapshotSpan? GetPreviousWord(this ITextStructureNavigator navigator, ITextView view)
        {
            return navigator.GetPreviousWord(view.GetCaretPosition());
        }

        internal static SnapshotSpan? GetPreviousWord(this ITextStructureNavigator navigator, SnapshotPoint position)
        {
            var textExtent = new TextExtent(new SnapshotSpan(position, 0), false);
            while (!textExtent.IsSignificant && textExtent.Span.Start.Position > 0)
                textExtent =
                    navigator.GetExtentOfWord(new SnapshotPoint(textExtent.Span.Snapshot,
                        textExtent.Span.Start.Position - 1));
            if (!textExtent.IsSignificant)
                return new SnapshotSpan?();
            return textExtent.Span;
        }

        internal static SnapshotSpan? GetNextWord(this ITextStructureNavigator navigator, ITextView view)
        {
            return navigator.GetNextWord(view.GetCaretPosition());
        }

        internal static SnapshotSpan? GetNextWord(this ITextStructureNavigator navigator, SnapshotPoint position)
        {
            TextExtent extentOfWord = navigator.GetExtentOfWord(position);
            while (!extentOfWord.IsSignificant && !extentOfWord.Span.IsEmpty)
                extentOfWord = navigator.GetExtentOfWord(extentOfWord.Span.End);
            if (!extentOfWord.IsSignificant)
                return new SnapshotSpan?();
            return extentOfWord.Span;
        }
    }
}