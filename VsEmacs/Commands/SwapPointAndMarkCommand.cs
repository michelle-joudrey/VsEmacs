namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.SwapPointAndMark)]
    internal class SwapPointAndMarkCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.MarkSession.SwapPointAndMark();
        }
    }
}