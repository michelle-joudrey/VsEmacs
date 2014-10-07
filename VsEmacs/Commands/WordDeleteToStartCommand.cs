using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.WordDeleteToStart, InverseCommand = EmacsCommandID.WordDeleteToEnd, UndoName = "Cut")]
    internal class WordDeleteToStartCommand : EmacsCommand
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
                        ? context.TextStructureNavigator.GetPreviousWord(context.TextView)
                        : context.TextStructureNavigator.GetPreviousWord(nullable.Value.Start);
                if (nullable.HasValue)
                {
                    SnapshotPoint caretPosition = context.TextView.GetCaretPosition();
                    context.EditorOperations.Delete(nullable.Value.Start, caretPosition - nullable.Value.Start);
                }
                KillwordClipboardSession clipboardSession =
                    context.Manager.GetOrCreateKillClipboardSession(context.TextView);
                clipboardSession.KillwordSession = changes + clipboardSession.KillwordSession;
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