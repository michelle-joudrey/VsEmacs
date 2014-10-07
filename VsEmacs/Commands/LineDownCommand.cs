namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.LineDown, CanBeRepeated = true, InverseCommand = EmacsCommandID.LineUp)]
    internal class LineDownCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.MoveLineDown();
        }
    }
}