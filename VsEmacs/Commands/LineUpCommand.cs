namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.LineUp, CanBeRepeated = true, InverseCommand = EmacsCommandID.LineDown)]
    internal class LineUpCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.MoveLineUp();
        }
    }
}