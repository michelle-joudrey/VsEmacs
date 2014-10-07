using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.WordTranspose, UndoName = "Transpose words")]
    internal class WordTransposeCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            SnapshotSpan? previousWord = context.TextStructureNavigator.GetPreviousWord(context.TextView);
            if (!previousWord.HasValue ||
                !previousWord.Value.IntersectsWith(new Span(context.TextView.GetCaretPosition(), 1)))
                return;
            SnapshotSpan? nextWord = context.TextStructureNavigator.GetNextWord(previousWord.Value.End);
            if (!nextWord.HasValue)
                return;
            string text1 = context.TextView.TextSnapshot.GetText(previousWord.Value);
            string text2 = context.TextView.TextSnapshot.GetText(nextWord.Value);
            using (ITextEdit edit = context.TextView.TextBuffer.CreateEdit())
            {
                edit.Replace(nextWord.Value, text1);
                edit.Replace(previousWord.Value, text2);
                edit.Apply();
            }
            context.TextView.Caret.MoveTo(new SnapshotPoint(context.TextView.TextSnapshot, nextWord.Value.End));
            context.TextView.Caret.EnsureVisible();
        }
    }
}