using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace VsEmacs
{
    internal static class ITextViewExtensions
    {
        internal static SnapshotPoint GetCaretPosition(this ITextView view)
        {
            return view.Caret.Position.BufferPosition;
        }
    }
}