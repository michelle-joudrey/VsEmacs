using System.Globalization;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.OptionsExtensionMethods;

namespace VsEmacs.Commands
{
    [EmacsCommand(VSConstants.VSStd2KCmdID.TAB, UndoName = "Indent")]
    internal class LineIndentCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            ITextSelection selection = context.TextView.Selection;
            bool flag = true;
            bool isActive = context.MarkSession.IsActive;
            if (context.TextBuffer.IsReadOnly(selection.Start.Position.GetContainingLine().Extent))
                return;
            if (!selection.IsEmpty)
            {
                if (selection.Mode == TextSelectionMode.Box)
                    return;
                VirtualSnapshotSpan streamSelectionSpan = selection.StreamSelectionSpan;
                if (streamSelectionSpan.Start.Position.GetContainingLine().LineNumber !=
                    streamSelectionSpan.End.Position.GetContainingLine().LineNumber)
                    return;
                selection.Clear();
                flag = false;
            }
            StripWhiteSpace(context.TextView.GetCaretPosition().GetContainingLine());
            int? desiredIndentation = context.Manager.SmartIndentationService.GetDesiredIndentation(context.TextView,
                context.TextView.GetCaretPosition().GetContainingLine());
            if (desiredIndentation.HasValue)
            {
                context.TextBuffer.Insert(context.TextView.GetCaretPosition().GetContainingLine().Start,
                    new string(' ', desiredIndentation.Value));
                if (!context.TextView.Options.IsConvertTabsToSpacesEnabled())
                    context.EditorOperations.ConvertSpacesToTabs();
            }
            else
            {
                int num = 0;
                if (flag)
                {
                    CaretPosition position = context.TextView.Caret.Position;
                    context.EditorOperations.MoveToStartOfLineAfterWhiteSpace(false);
                    num = position.BufferPosition.Position - context.TextView.GetCaretPosition();
                }
                context.EditorOperations.SelectAndMoveCaret(
                    new VirtualSnapshotPoint(context.TextView.GetCaretPosition().GetContainingLine().Start, 0),
                    new VirtualSnapshotPoint(context.TextView.GetCaretPosition().GetContainingLine().End, 0));
                context.CommandRouter.ExecuteDTECommand("Edit.FormatSelection");
                context.EditorOperations.MoveToStartOfLineAfterWhiteSpace(false);
                if (num > 0)
                    context.EditorOperations.MoveCaret(context.TextView.Caret.Position.BufferPosition + num, false);
            }
            if (isActive)
                return;
            context.MarkSession.Deactivate(true);
        }

        private void StripWhiteSpace(ITextSnapshotLine line)
        {
            ITextSnapshot snapshot = line.Snapshot;
            ITextBuffer textBuffer = snapshot.TextBuffer;
            int position = line.Start.Position;
            while (position < line.End.Position && IsSpaceCharacter(snapshot[position]))
                ++position;
            int index = line.End.Position - 1;
            while (index > position && IsSpaceCharacter(snapshot[index]))
                --index;
            if (index == line.End.Position - 1 && position == line.Start.Position)
                return;
            using (ITextEdit edit = textBuffer.CreateEdit())
            {
                edit.Delete(Span.FromBounds(index + 1, line.End.Position));
                edit.Delete(Span.FromBounds(line.Start.Position, position));
                edit.Apply();
            }
        }

        private static bool IsSpaceCharacter(char c)
        {
            if (c != 9 && c != 8203)
                return char.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator;
            return true;
        }
    }
}