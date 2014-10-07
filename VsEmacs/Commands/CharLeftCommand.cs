namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.CharLeft, CanBeRepeated = true, InverseCommand = EmacsCommandID.CharRight)]
    internal class CharLeftCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.MoveToPreviousCharacter();
        }
    }
}