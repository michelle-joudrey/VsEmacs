using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(VSConstants.VSStd97CmdID.Paste, UndoName = "Paste")]
    internal class PasteCommand : EmacsCommand
    {
        internal static Span LastPastedSpan;

        internal override void Execute(EmacsCommandContext context)
        {
            if (context.Manager.ClipboardRing.Count <= 0)
                return;
            context.MarkSession.PushMark(true);
            LastPastedSpan = new Span(context.TextView.Caret.Position.BufferPosition.Position,
                context.Manager.ClipboardRing.Last().Length);
            context.EditorOperations.InsertText(context.Manager.ClipboardRing.Last());
            context.Manager.ClipboardRingIndex = context.Manager.ClipboardRing.Count - 1;
            context.Manager.GetOrCreateKillClipboardSession(context.TextView).KillwordSession = string.Empty;
        }
    }
}