namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.Quit)]
    internal class QuitCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.Manager.ClearStatus();
        }
    }
}