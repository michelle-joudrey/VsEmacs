using Microsoft.VisualStudio;

namespace VsEmacs.Commands
{
    [EmacsCommand(VSConstants.VSStd97CmdID.Delete, CanBeRepeated = true, UndoName = "Delete")]
    internal class DeleteCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.EditorOperations.Delete();
        }

        internal override void ExecuteInverse(EmacsCommandContext context)
        {
            context.EditorOperations.Backspace();
        }
    }
}