namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.DocumentStart)]
    internal class DocumentStartCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.MarkSession.PushMark(false);
            context.EditorOperations.MoveToStartOfDocument();
        }
    }
}