namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.FindReplace)]
    internal class FindReplaceCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.CommandRouter.ExecuteDTECommand("Edit.Replace");
        }
    }
}