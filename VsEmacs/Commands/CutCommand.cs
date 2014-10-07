using Microsoft.VisualStudio;

namespace VsEmacs.Commands
{
    [EmacsCommand(VSConstants.VSStd97CmdID.Cut, CopyDeletedTextToTheClipboard = true, UndoName = "Delete")]
    internal class CutCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            if (!context.TextView.Selection.IsEmpty)
                context.EditorOperations.CutSelection();
            else
                context.Manager.UpdateStatus(Resources.OperationCannotBePerformedWithoutTextSelection, false);
        }
    }
}