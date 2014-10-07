using System;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using VsEmacs.Commands;

namespace VsEmacs
{
    internal class KillwordClipboardSession : IOleCommandTarget
    {
        private readonly EmacsCommandsManager manager;
        private string killwordSession;
        private ITextView view;

        public KillwordClipboardSession(ITextView view, EmacsCommandsManager manager)
        {
            this.view = view;
            this.manager = manager;
        }

        public string KillwordSession
        {
            get { return killwordSession; }
            set { killwordSession = value; }
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (manager.IsEnabled)
            {
                if (pguidCmdGroup == typeof (EmacsCommandID).GUID)
                {
                    if ((int) nCmdID == 32 || (int) nCmdID == 33)
                        manager.ClipboardRing.Remove(manager.ClipboardRing.LastOrDefault());
                    if ((int) nCmdID == 7 || (int) nCmdID == 8)
                        killwordSession = string.Empty;
                }
                if (pguidCmdGroup == typeof (VSConstants.VSStd97CmdID).GUID && (int) nCmdID == 153)
                    killwordSession = string.Empty;
                if (pguidCmdGroup == typeof (VSConstants.VSStd2KCmdID).GUID &&
                    ((int) nCmdID == 2 || (int) nCmdID == 6 || ((int) nCmdID == 11 || (int) nCmdID == 13) ||
                     ((int) nCmdID == 29 || (int) nCmdID == 27 || ((int) nCmdID == 7 || (int) nCmdID == 9)) ||
                     (int) nCmdID == 1))
                    killwordSession = string.Empty;
            }
            return 1;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return 1;
        }
    }
}