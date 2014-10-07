﻿using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;

namespace VsEmacs
{
    internal static class IEditorOperationsExtensions
    {
        private static bool ShouldExtendSelection(IEditorOperations editorOperations)
        {
            return MarkSession.GetSession(editorOperations.TextView).IsActive;
        }

        internal static void MoveToTopOfView(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToTopOfView(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveToBottomOfView(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToBottomOfView(ShouldExtendSelection(editorOperations));
        }

        internal static void PageDown(this IEditorOperations editorOperations)
        {
            editorOperations.PageDown(ShouldExtendSelection(editorOperations));
        }

        internal static void PageUp(this IEditorOperations editorOperations)
        {
            editorOperations.PageUp(ShouldExtendSelection(editorOperations));
        }

        internal static void DeleteToEndOfPhysicalLine(this IEditorOperations editorOperations)
        {
            int position = editorOperations.TextView.GetCaretPosition().Position;
            editorOperations.Delete(position, editorOperations.GetCaretPhysicalLine().End - position);
        }

        internal static void DeleteToBeginningOfPhysicalLine(this IEditorOperations editorOperations)
        {
            int position = editorOperations.TextView.GetCaretPosition().Position;
            editorOperations.Delete(editorOperations.GetCaretPhysicalLine().Start,
                position - editorOperations.GetCaretPhysicalLine().Start);
        }

        internal static ITextSnapshotLine GetCaretPhysicalLine(this IEditorOperations editorOperations)
        {
            SnapshotPoint caretPosition = editorOperations.TextView.GetCaretPosition();
            return editorOperations.TextView.TextSnapshot.TextBuffer.GetContainingLine(caretPosition);
        }

        internal static void MoveCaretToStartOfPhysicalLine(this IEditorOperations editorOperations)
        {
            editorOperations.MoveCaretToStartOfPhysicalLine(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveCaretToStartOfPhysicalLine(this IEditorOperations editorOperations,
            bool extendSelection)
        {
            editorOperations.MoveCaret(editorOperations.GetCaretPhysicalLine().Start, extendSelection);
        }

        internal static void MoveCaretToEndOfPhysicalLine(this IEditorOperations editorOperations)
        {
            editorOperations.MoveCaretToEndOfPhysicalLine(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveCaretToEndOfPhysicalLine(this IEditorOperations editorOperations, bool extendSelection)
        {
            editorOperations.MoveCaret(editorOperations.GetCaretPhysicalLine().End, extendSelection);
        }

        internal static void MoveCaret(this IEditorOperations editorOperations, int position)
        {
            editorOperations.MoveCaret(new SnapshotPoint(editorOperations.TextView.TextSnapshot, position));
        }

        internal static void MoveCaret(this IEditorOperations editorOperations, SnapshotPoint bufferPosition)
        {
            editorOperations.MoveCaret(bufferPosition, ShouldExtendSelection(editorOperations));
        }

        internal static void MoveCaret(this IEditorOperations editorOperations, SnapshotPoint bufferPosition,
            bool extendSelection)
        {
            if (extendSelection)
            {
                VirtualSnapshotPoint anchorPoint = editorOperations.TextView.Selection.AnchorPoint;
                editorOperations.TextView.Caret.MoveTo(bufferPosition);
                editorOperations.TextView.Selection.Select(
                    anchorPoint.TranslateTo(editorOperations.TextView.TextSnapshot),
                    editorOperations.TextView.Caret.Position.VirtualBufferPosition);
            }
            else
            {
                editorOperations.TextView.Selection.Clear();
                editorOperations.TextView.Caret.MoveTo(bufferPosition);
            }
            editorOperations.TextView.Caret.EnsureVisible();
        }

        internal static void MoveToStartOfDocument(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToStartOfDocument(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveToEndOfDocument(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToEndOfDocument(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveToNextCharacter(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToNextCharacter(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveToPreviousCharacter(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToPreviousCharacter(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveToEndOfLine(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToEndOfLine(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveToStartOfLine(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToStartOfLine(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveLineUp(this IEditorOperations editorOperations)
        {
            editorOperations.MoveLineUp(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveLineDown(this IEditorOperations editorOperations)
        {
            editorOperations.MoveLineDown(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveToNextWord(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToNextWord(ShouldExtendSelection(editorOperations));
        }

        internal static void MoveToPreviousWord(this IEditorOperations editorOperations)
        {
            editorOperations.MoveToPreviousWord(ShouldExtendSelection(editorOperations));
        }

        internal static void Delete(this IEditorOperations editorOperations, int start, int length)
        {
            editorOperations.Delete(new Span(start, length));
        }

        internal static void Delete(this IEditorOperations editorOperations, Span span)
        {
            editorOperations.TextView.TextBuffer.Delete(span);
        }
    }
}