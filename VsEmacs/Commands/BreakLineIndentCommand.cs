namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.BreakLineIndent, CanBeRepeated = true, UndoName = "Indent")]
    internal class BreakLineIndentCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.CommandRouter.ExecuteDTECommand("Edit.BreakLine");
        }

        internal override void ExecuteInverse(EmacsCommandContext context)
        {
        }
    }
}