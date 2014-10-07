namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.ActivateRegion)]
    internal class ActivateRegionCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.MarkSession.Activate();
        }
    }
}