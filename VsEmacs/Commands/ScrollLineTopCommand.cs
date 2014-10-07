namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.ScrollLineTop)]
    internal class ScrollLineTopCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.ScrollLineTop();
        }
    }
}