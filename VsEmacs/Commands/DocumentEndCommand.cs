namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.DocumentEnd)]
    internal class DocumentEndCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.MarkSession.PushMark(false);
            context.EditorOperations.MoveToEndOfDocument();
        }
    }
}