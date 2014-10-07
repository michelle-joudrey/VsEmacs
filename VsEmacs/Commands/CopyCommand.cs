using Microsoft.VisualStudio;

namespace VsEmacs.Commands
{
    [EmacsCommand(VSConstants.VSStd97CmdID.Copy)]
    internal class CopyCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            if (!context.TextView.Selection.IsEmpty)
                context.Clipboard.Append(context.EditorOperations.SelectedText);
            else
                context.Manager.UpdateStatus("The region is not active", false);
        }
    }
}