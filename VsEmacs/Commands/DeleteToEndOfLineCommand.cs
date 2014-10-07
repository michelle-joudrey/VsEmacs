using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.DeleteToEndOfLine, CopyDeletedTextToTheClipboard = true, UndoName = "Cut")]
    internal class DeleteToEndOfLineCommand : EmacsCommand
    {
        private StringBuilder changes;

        internal override void Execute(EmacsCommandContext context)
        {
            try
            {
                KillwordClipboardSession clipboardSession =
                    context.Manager.GetOrCreateKillClipboardSession(context.TextView);
                changes = new StringBuilder();
                context.TextBuffer.Changed += OnTextBufferChanged;
                int? universalArgument1 = context.Manager.UniversalArgument;
                if ((universalArgument1.GetValueOrDefault() != 0 ? 0 : (universalArgument1.HasValue ? 1 : 0)) != 0)
                {
                    context.EditorOperations.DeleteToBeginningOfPhysicalLine();
                    clipboardSession.KillwordSession = changes + clipboardSession.KillwordSession;
                }
                else
                {
                    if (context.UniversalArgument.HasValue)
                    {
                        int? universalArgument2 = context.UniversalArgument;
                        if ((universalArgument2.GetValueOrDefault() <= 0 ? 0 : (universalArgument2.HasValue ? 1 : 0)) ==
                            0)
                            goto label_11;
                    }
                    for (int argumentOrDefault = context.Manager.GetUniversalArgumentOrDefault(1);
                        argumentOrDefault > 0 && context.TextView.Caret.ContainingTextViewLine != null;
                        --argumentOrDefault)
                    {
                        if (context.TextView.GetCaretPosition() ==
                            context.TextView.Caret.ContainingTextViewLine.End.Position)
                            context.EditorOperations.Delete();
                        else
                            context.EditorOperations.DeleteToEndOfPhysicalLine();
                    }
                    clipboardSession.KillwordSession = clipboardSession.KillwordSession + changes;
                }
                label_11:
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