using EnvDTE;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;

namespace VsEmacs
{
    internal class CommandRouter : IOleCommandTarget
    {
        private readonly ICompletionBroker _completionBroker;
        private readonly DTE _dte;
        private readonly LinkedList<IOleCommandTarget> _targets;
        private readonly ITextView _view;
        private readonly IOleCommandTarget _viewCommandTarget;
        private bool _inExecute;

        public CommandRouter(ITextView view, IOleCommandTarget viewCommandTarget, ICompletionBroker completionBroker,
            DTE dte)
        {
            _targets = new LinkedList<IOleCommandTarget>();
            _view = view;
            _completionBroker = completionBroker;
            _inExecute = false;
            _viewCommandTarget = viewCommandTarget;
            _dte = dte;
        }

        public IOleCommandTarget Next { get; set; }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (IsAllowed(ref pguidCmdGroup, nCmdID))
            {
                foreach (IOleCommandTarget oleCommandTarget in _targets)
                {
                    if (oleCommandTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut) == 0)
                        return 0;
                }
            }
            return Next.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (IsAllowed(ref pguidCmdGroup, prgCmds[0].cmdID))
            {
                foreach (IOleCommandTarget oleCommandTarget in _targets)
                {
                    if (oleCommandTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText) == 0)
                        return 0;
                }
            }
            return Next.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int ExecuteCommand(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            Guid commandGroupCopy = pguidCmdGroup;
            return
                ExecuteClosuredCommand(
                    () => _viewCommandTarget.Exec(ref commandGroupCopy, nCmdID, nCmdexecopt, pvaIn, pvaOut));
        }

        public void ExecuteDTECommand(string visualStudioCommandName)
        {
            ExecuteClosuredCommand(() =>
            {
                if (_dte != null)
                    _dte.ExecuteCommand(visualStudioCommandName, "");
                return (object) null;
            });
        }

        public void AddCommandTarget(IOleCommandTarget target)
        {
            _targets.AddFirst(target);
        }

        private ReturnType ExecuteClosuredCommand<ReturnType>(Func<ReturnType> command)
        {
            if (_inExecute)
                throw new InvalidOperationException("Already executing closured command. The command filter has a loop");
            try
            {
                _inExecute = true;
                return command();
            }
            finally
            {
                _inExecute = false;
            }
        }

        private bool IsAllowed(ref Guid pguidCmdGroup, uint nCmdID)
        {
            if (_inExecute)
                return false;
            if (IsIntellisenseActive())
                return !IsIntellisenseCommand(ref pguidCmdGroup, nCmdID);
            return true;
        }

        private bool IsIntellisenseActive()
        {
            return _completionBroker.IsCompletionActive(_view);
        }

        private bool IsIntellisenseCommand(ref Guid pguidCmdGroup, uint nCmdID)
        {
            if (pguidCmdGroup == VSConstants.VSStd2K)
            {
                uint num = nCmdID;
                if (num <= 103U)
                {
                    if (num <= 67U)
                    {
                        switch (num)
                        {
                            case 1U:
                            case 2U:
                            case 3U:
                            case 4U:
                            case 6U:
                            case 7U:
                            case 8U:
                            case 9U:
                            case 10U:
                            case 11U:
                            case 12U:
                            case 13U:
                            case 14U:
                            case 19U:
                            case 20U:
                            case 21U:
                            case 22U:
                            case 23U:
                            case 24U:
                            case 27U:
                            case 28U:
                            case 29U:
                            case 30U:
                            case 31U:
                            case 32U:
                            case 33U:
                            case 34U:
                            case 67U:
                                break;
                            default:
                                goto label_11;
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 93U:
                            case 94U:
                            case 96U:
                            case 97U:
                            case 103U:
                                break;
                            default:
                                goto label_11;
                        }
                    }
                }
                else if (num <= 146U)
                {
                    switch (num)
                    {
                        case 117U:
                        case 118U:
                        case 145U:
                        case 146U:
                            break;
                        default:
                            goto label_11;
                    }
                }
                else if ((int) num != 150 && (int) num != 2303)
                    goto label_11;
                return true;
            }
            if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97 && (int) nCmdID == 17)
                return true;
            label_11:
            return false;
        }
    }
}