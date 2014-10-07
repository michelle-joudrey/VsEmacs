using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using VsEmacs.Commands;

namespace VsEmacs
{
    internal class UniversalArgumentSession : IOleCommandTarget
    {
        private const char NegativeArgumentSign = '-';
        private const int DefaultUniversalValue = 4;
        private readonly EmacsCommandsManager manager;
        private StringBuilder universalArgumentString;
        private ITextView view;

        internal UniversalArgumentSession(ITextView view, EmacsCommandsManager manager)
        {
            this.view = view;
            this.manager = manager;
        }

        private bool IsActive { get; set; }

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (manager.IsEnabled)
            {
                if (IsActive)
                {
                    if (pguidCmdGroup == VSConstants.VSStd2K)
                    {
                        if ((int) nCmdID == 1)
                        {
                            var c = (char) (ushort) Marshal.GetObjectForNativeVariant(pvaIn);
                            if (c == 45 && universalArgumentString.Length == 0 || char.IsNumber(c))
                            {
                                manager.UpdateStatus(c.ToString(), true);
                                universalArgumentString.Append(c);
                                return 0;
                            }
                            Commit(true);
                        }
                        else if ((int) nCmdID == 2)
                            Commit(true);
                    }
                    else if (pguidCmdGroup == typeof (VSConstants.VSStd97CmdID).GUID && (int) nCmdID == 17)
                        Commit(true);
                    if (pguidCmdGroup == typeof (EmacsCommandID).GUID)
                    {
                        if ((int) nCmdID == 51)
                            Cancel();
                        else
                            Commit((int) nCmdID != 52);
                    }
                }
                else if (manager.UniversalArgument.HasValue)
                    manager.UniversalArgument = new int?();
            }
            return 1;
        }

        int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return 1;
        }

        private void Commit(bool deactivate = true)
        {
            int result = int.MinValue;
            if (!int.TryParse(universalArgumentString.ToString(), out result))
            {
                if (universalArgumentString.Length == 0)
                    result = 4;
                else if (universalArgumentString.ToString() == '-'.ToString())
                    result = -4;
            }
            if (result != int.MinValue)
            {
                if (!manager.UniversalArgument.HasValue)
                    manager.UniversalArgument = 1;
                EmacsCommandsManager emacsCommandsManager = manager;
                int? universalArgument = manager.UniversalArgument;
                int num = result;
                int? nullable = universalArgument.HasValue ? universalArgument.GetValueOrDefault()*num : new int?();
                emacsCommandsManager.UniversalArgument = nullable;
            }
            if (!deactivate)
                return;
            manager.ClearStatus();
            IsActive = false;
        }

        private void Cancel()
        {
            manager.UniversalArgument = new int?();
            manager.ClearStatus();
            IsActive = false;
        }

        internal void Start()
        {
            universalArgumentString = new StringBuilder();
            manager.UpdateStatus(" c-u ", IsActive);
            IsActive = true;
        }

        internal static UniversalArgumentSession GetSession(ITextView view)
        {
            return view.Properties.GetProperty<UniversalArgumentSession>(typeof (UniversalArgumentSession));
        }
    }
}