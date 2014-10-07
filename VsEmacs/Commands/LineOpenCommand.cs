using System;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.LineOpen, CanBeRepeated = true, UndoName = "Enter")]
    internal class LineOpenCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.InsertText(Environment.NewLine);
            context.EditorOperations.MoveToPreviousCharacter();
        }
    }
}