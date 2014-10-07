namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.ScrollPageDown, CanBeRepeated = true, InverseCommand = EmacsCommandID.ScrollPageUp)]
    internal class ScrollPageDownCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            int viewLine = context.TextBuffer.GetLineNumber(context.TextView.Caret.Position.BufferPosition) -
                           context.TextBuffer.GetLineNumber(context.TextView.TextViewLines.FirstVisibleLine.Start);
            context.EditorOperations.ScrollPageDown();
            context.TextView.PositionCaretOnLine(viewLine);
        }
    }
}