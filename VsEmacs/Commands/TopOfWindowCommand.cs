namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.TopOfWindow)]
    internal class TopOfWindowCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.MoveToTopOfView();
        }
    }
}