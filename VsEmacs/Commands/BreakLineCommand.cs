namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.BreakLine, CanBeRepeated = true, UndoName = "Enter")]
    internal class BreakLineCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            int num = context.TextView.TextViewLines.IndexOf(context.TextView.Caret.ContainingTextViewLine);
            context.CommandRouter.ExecuteDTECommand("Edit.BreakLine");
            if (num == context.TextView.TextViewLines.IndexOf(context.TextView.Caret.ContainingTextViewLine))
                return;
            context.EditorOperations.MoveToStartOfLine();
        }

        internal override void ExecuteInverse(EmacsCommandContext context)
        {
        }
    }
}