using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using VsEmacs.Commands;

namespace VsEmacs
{
    internal class EmacsCommandsFilter : IOleCommandTarget
    {
        private readonly EmacsCommandsManager manager;
        private readonly CommandRouter router;
        private readonly ITextView view;

        public EmacsCommandsFilter(ITextView view, EmacsCommandsManager manager, CommandRouter router)
        {
            this.view = view;
            this.manager = manager;
            this.router = router;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (manager.IsEnabled)
            {
                IEmacsCommandMetadata commandMetadata = manager.GetCommandMetadata((int) nCmdID, pguidCmdGroup);
                if (commandMetadata != null)
                {
                    if (commandMetadata != null)
                    {
                        try
                        {
                            manager.Execute(view, commandMetadata, true);
                            manager.AfterSearch = false;
                        }
                        catch (Exception ex)
                        {
                            manager.UpdateStatus(ex.Message, false);
                            return 1;
                        }
                    }
                    return 0;
                }
                if (pguidCmdGroup == VSConstants.VSStd2K && (int) nCmdID == 1 && manager.UniversalArgument.HasValue &&
                    manager.UniversalArgument.Value > 1)
                {
                    int num1 = manager.UniversalArgument.Value;
                    while (num1-- > 0)
                    {
                        int num2 = router.ExecuteCommand(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
                        if (num2 != 0)
                            return num2;
                    }
                    return 0;
                }
                if (pguidCmdGroup == VSConstants.VSStd2K && ((int) nCmdID == 122 || (int) nCmdID == 123))
                {
                    MarkSession.GetSession(view).PushMark(true);
                    manager.AfterSearch = true;
                }
            }
            return 1;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (!manager.IsEnabled || !(pguidCmdGroup == typeof (EmacsCommandID).GUID) ||
                (cCmds <= 0U || manager.GetCommandMetadata((int) prgCmds[0].cmdID, pguidCmdGroup) == null))
                return 1;
            prgCmds[0].cmdf = 3U;
            return 0;
        }
    }
}