namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.LineEnd)]
    internal class LineEndCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            if (context.Manager.AfterSearch)
                context.EditorOperations.MoveCaretToEndOfPhysicalLine(false);
            else
                context.EditorOperations.MoveCaretToEndOfPhysicalLine();
        }
    }
}