namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.ScrollPageUp, CanBeRepeated = true, InverseCommand = EmacsCommandID.ScrollPageDown)]
    internal class ScrollPageUpCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            int viewLine = context.TextBuffer.GetLineNumber(context.TextView.Caret.Position.BufferPosition) -
                           context.TextBuffer.GetLineNumber(context.TextView.TextViewLines.FirstVisibleLine.Start);
            context.EditorOperations.ScrollPageUp();
            context.TextView.PositionCaretOnLine(viewLine);
        }
    }
}