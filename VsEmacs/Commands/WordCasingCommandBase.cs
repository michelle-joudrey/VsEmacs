using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    internal abstract class WordCasingCommandBase : EmacsCommand
    {
        internal abstract string TransformText(string text);

        internal override void Execute(EmacsCommandContext context)
        {
            SnapshotSpan? nextWord = context.TextStructureNavigator.GetNextWord(context.TextView);
            if (!nextWord.HasValue)
                return;
            SnapshotPoint caretPosition = context.TextView.GetCaretPosition();
            var span = new Span(caretPosition, nextWord.Value.End - caretPosition);
            string text = context.TextView.TextSnapshot.GetText(span);
            context.TextBuffer.Replace(span, TransformText(text));
        }

        internal override void ExecuteInverse(EmacsCommandContext context)
        {
            SnapshotSpan? previousWord = context.TextStructureNavigator.GetPreviousWord(context.TextView);
            if (!previousWord.HasValue)
                return;
            SnapshotPoint caretPosition = context.TextView.GetCaretPosition();
            var span = new Span(previousWord.Value.Start, caretPosition - previousWord.Value.Start);
            string text = context.TextView.TextSnapshot.GetText(span);
            context.TextBuffer.Replace(span, TransformText(text));
            context.EditorOperations.MoveCaret(span.Start);
        }
    }
}