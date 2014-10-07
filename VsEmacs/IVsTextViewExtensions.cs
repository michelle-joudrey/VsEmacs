using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.TextManager.Interop;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace VsEmacs
{
    internal static class IVsTextViewExtensions
    {
        internal static IVsWindowFrame GetWinfowFrame(this IVsTextView textView)
        {
            var objectWithSite = (IObjectWithSite) textView;
            Guid guid1 = typeof (IServiceProvider).GUID;
            IntPtr ppvSite;
            objectWithSite.GetSite(ref guid1, out ppvSite);
            var serviceProvider = (IServiceProvider) Marshal.GetObjectForIUnknown(ppvSite);
            Marshal.Release(ppvSite);
            Guid guid2 = typeof (SVsWindowFrame).GUID;
            Guid guid3 = typeof (IVsWindowFrame).GUID;
            IntPtr ppvObject;
            serviceProvider.QueryService(ref guid2, ref guid3, out ppvObject);
            var vsWindowFrame = (IVsWindowFrame) Marshal.GetObjectForIUnknown(ppvObject);
            Marshal.Release(ppvObject);
            return vsWindowFrame;
        }

        internal static int GetStartPositionAfterLines(this ITextView textView, ITextViewLine line,
            int currentLineDifference)
        {
            int lineNumber = textView.TextBuffer.GetLineNumber(line.Start) + currentLineDifference;
            if (lineNumber >= textView.TextBuffer.CurrentSnapshot.LineCount)
                lineNumber = textView.TextBuffer.CurrentSnapshot.LineCount - 1;
            return textView.TextSnapshot.GetLineFromLineNumber(lineNumber).Start.Position;
        }

        internal static void PositionCaretOnLine(this ITextView textView, int viewLine)
        {
            int position = textView.GetStartPositionAfterLines(textView.TextViewLines.FirstVisibleLine, viewLine);
            if (position >= textView.TextSnapshot.Length)
                position = textView.TextSnapshot.Length - 1;
            textView.Caret.MoveTo(new SnapshotPoint(textView.TextSnapshot, position));
            textView.Caret.EnsureVisible();
        }
    }
}