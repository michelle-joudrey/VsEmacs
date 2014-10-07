using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using VsEmacs.Commands;

namespace VsEmacs
{
    internal class InteractiveRoleWorkAroundFilter : IOleCommandTarget
    {
        private EmacsCommandsManager manager;
        private ITextView view;

        public InteractiveRoleWorkAroundFilter(ITextView view, EmacsCommandsManager manager)
        {
            this.view = view;
            this.manager = manager;
        }

        internal IOleCommandTarget NextCommandTarget { get; set; }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == typeof (EmacsCommandID).GUID && (int) nCmdID == 21)
            {
                Guid guid = typeof (VSConstants.VSStd2KCmdID).GUID;
                return NextCommandTarget.Exec(ref guid, 3U, nCmdexecopt, pvaIn, pvaOut);
            }
            Guid pguidCmdGroup1 = pguidCmdGroup;
            return NextCommandTarget.Exec(ref pguidCmdGroup1, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup == typeof (EmacsCommandID).GUID && cCmds > 0U && (int) prgCmds[0].cmdID == 21)
            {
                prgCmds[0].cmdf = 3U;
                return 0;
            }
            Guid pguidCmdGroup1 = pguidCmdGroup;
            return NextCommandTarget.QueryStatus(ref pguidCmdGroup1, cCmds, prgCmds, pCmdText);
        }
    }
}