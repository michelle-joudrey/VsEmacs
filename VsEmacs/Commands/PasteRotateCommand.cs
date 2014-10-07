using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.PasteRotate, UndoName = "Paste")]
    internal class PasteRotateCommand : EmacsCommand
    {
        internal static Span LastPastedSpan;

        internal override void Execute(EmacsCommandContext context)
        {
            if (!CheckLastInvokedCommand(context))
                throw new NoOperationException();
            --context.Manager.ClipboardRingIndex;
            if (context.Manager.ClipboardRingIndex < 0)
                context.Manager.ClipboardRingIndex = context.Manager.ClipboardRing.Count - 1;
            if (context.Manager.ClipboardRingIndex == -1)
                return;
            string text = context.Manager.ClipboardRing[context.Manager.ClipboardRingIndex];
            context.EditorOperations.ReplaceText(LastPastedSpan, text);
            LastPastedSpan = new Span(LastPastedSpan.Start, text.Length);
            context.TextView.Selection.Select(new SnapshotSpan(context.TextView.TextSnapshot, LastPastedSpan), false);
        }

        private bool CheckLastInvokedCommand(EmacsCommandContext context)
        {
            if (context.Manager.LastExecutedCommand != null)
            {
                if (context.Manager.LastExecutedCommand.Command == 26 &&
                    new Guid(context.Manager.LastExecutedCommand.CommandGroup) == typeof (VSConstants.VSStd97CmdID).GUID)
                {
                    LastPastedSpan = PasteCommand.LastPastedSpan;
                    return true;
                }
                if (context.Manager.LastExecutedCommand.Command == 37 &&
                    new Guid(context.Manager.LastExecutedCommand.CommandGroup) == typeof (EmacsCommandID).GUID)
                    return true;
            }
            return false;
        }
    }
}