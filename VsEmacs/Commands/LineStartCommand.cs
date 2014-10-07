namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.LineStart)]
    internal class LineStartCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            if (context.Manager.AfterSearch)
                context.EditorOperations.MoveCaretToStartOfPhysicalLine(false);
            else
                context.EditorOperations.MoveCaretToStartOfPhysicalLine();
        }
    }
}