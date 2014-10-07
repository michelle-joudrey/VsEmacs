using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.WordDeleteToEnd, InverseCommand = EmacsCommandID.WordDeleteToStart, UndoName = "Cut")]
    internal class WordDeleteToEndCommand : EmacsCommand
    {
        private StringBuilder changes;

        internal override void Execute(EmacsCommandContext context)
        {
            try
            {
                changes = new StringBuilder();
                context.TextBuffer.Changed += OnTextBufferChanged;
                var nullable = new SnapshotSpan?();
                for (int argumentOrDefault = context.Manager.GetUniversalArgumentOrDefault(1);
                    argumentOrDefault > 0;
                    --argumentOrDefault)
                    nullable = !nullable.HasValue
                        ? context.TextStructureNavigator.GetNextWord(context.TextView)
                        : context.TextStructureNavigator.GetNextWord(nullable.Value.End);
                if (nullable.HasValue)
                {
                    SnapshotPoint caretPosition = context.TextView.GetCaretPosition();
                    context.EditorOperations.Delete(caretPosition, nullable.Value.End - caretPosition);
                }
                KillwordClipboardSession clipboardSession =
                    context.Manager.GetOrCreateKillClipboardSession(context.TextView);
                clipboardSession.KillwordSession = clipboardSession.KillwordSession + changes;
                context.Clipboard.Clear();
                context.Clipboard.Append(clipboardSession.KillwordSession);
            }
            finally
            {
                context.TextBuffer.Changed -= OnTextBufferChanged;
            }
        }

        private void OnTextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            e.Changes.ToList().ForEach(change => changes.Append(change.OldText));
        }
    }
}