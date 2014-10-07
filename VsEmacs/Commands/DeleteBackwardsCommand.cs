using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(VSConstants.VSStd2KCmdID.BACKSPACE, CanBeRepeated = true, UndoName = "Delete")]
    internal class DeleteBackwardsCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            int position = context.TextView.GetCaretPosition().Position;
            if (position <= 0)
                return;
            if (context.TextBuffer.CurrentSnapshot.GetText(context.TextView.GetCaretPosition() - 1, 1) == "\t")
            {
                context.TextView.Selection.Select(
                    new SnapshotSpan(context.TextView.TextSnapshot, new Span(position - 1, 1)), false);
                context.EditorOperations.ConvertTabsToSpaces();
                context.MarkSession.Deactivate(true);
                context.EditorOperations.Backspace();
            }
            else
                context.EditorOperations.Backspace();
        }

        internal override void ExecuteInverse(EmacsCommandContext context)
        {
            if (context.TextView.GetCaretPosition().Position >= context.TextBuffer.CurrentSnapshot.Length)
                return;
            context.EditorOperations.Delete();
        }
    }
}