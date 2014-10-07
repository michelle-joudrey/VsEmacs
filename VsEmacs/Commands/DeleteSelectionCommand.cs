namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.DeleteSelection, UndoName = "Delete")]
    internal class DeleteSelectionCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            if (!context.TextView.Selection.IsEmpty)
                context.EditorOperations.Delete();
            else
                context.Manager.UpdateStatus(Resources.OperationCannotBePerformedWithoutTextSelection, false);
        }
    }
}