namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.CharRight, CanBeRepeated = true, InverseCommand = EmacsCommandID.CharLeft)]
    internal class CharRightCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.MoveToNextCharacter();
        }
    }
}