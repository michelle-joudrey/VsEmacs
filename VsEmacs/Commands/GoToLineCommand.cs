namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.GoToLine)]
    internal class GoToLineCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            if (context.Manager.UniversalArgument.HasValue)
            {
                int lineNumber = context.Manager.UniversalArgument.Value - 1;
                if (lineNumber < 0)
                    context.EditorOperations.MoveToStartOfDocument();
                else if (lineNumber >= context.TextView.TextSnapshot.LineCount)
                    context.EditorOperations.MoveToEndOfDocument();
                else
                    context.EditorOperations.GotoLine(lineNumber);
            }
            else
                context.CommandRouter.ExecuteDTECommand("Edit.GoTo");
        }
    }
}