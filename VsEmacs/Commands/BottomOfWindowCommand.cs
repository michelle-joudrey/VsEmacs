namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.BottomOfWindow)]
    internal class BottomOfWindowCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.MoveToBottomOfView();
        }
    }
}