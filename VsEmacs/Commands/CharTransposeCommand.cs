namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.CharTranspose, UndoName = "Transpose characters")]
    internal class CharTransposeCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.TransposeCharacter();
        }
    }
}