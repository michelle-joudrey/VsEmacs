using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using VsEmacs.Commands;

namespace VsEmacs
{
    internal class MarkSession : MouseProcessorBase, IOleCommandTarget
    {
        private readonly EmacsCommandsManager manager;
        private readonly Stack<ITrackingPoint> marks = new Stack<ITrackingPoint>();
        private readonly ITextView view;
        private ITrackingPoint activeMark;
        private ITrackingPoint currentMark;

        internal MarkSession(ITextView view, EmacsCommandsManager manager)
        {
            this.manager = manager;
            this.view = view;
            this.view.Selection.SelectionChanged += Selection_SelectionChanged;
            activeMark = currentMark = CreateTrackingPoint(0);
        }

        internal bool IsActive { get; private set; }

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (IsActive && manager.IsEnabled)
            {
                if (pguidCmdGroup == VSConstants.VSStd2K)
                {
                    switch (nCmdID)
                    {
                        case 1U:
                        case 2U:
                        case 4U:
                        case 103U:
                            Deactivate(true);
                            break;
                        case 7U:
                            manager.Execute(view, EmacsCommandID.CharLeft, true);
                            return 0;
                        case 9U:
                            manager.Execute(view, EmacsCommandID.CharRight, true);
                            return 0;
                        case 11U:
                            manager.Execute(view, EmacsCommandID.LineUp, true);
                            return 0;
                        case 13U:
                            manager.Execute(view, EmacsCommandID.LineDown, true);
                            return 0;
                    }
                }
                else if (pguidCmdGroup == typeof (VSConstants.VSStd97CmdID).GUID)
                {
                    switch (nCmdID)
                    {
                        case 15U:
                        case 16U:
                            Deactivate(false);
                            break;
                        case 17U:
                            Deactivate(true);
                            break;
                    }
                }
                else if (pguidCmdGroup == typeof (EmacsCommandID).GUID)
                {
                    switch (nCmdID)
                    {
                        case 21U:
                        case 22U:
                        case 34U:
                        case 51U:
                            Deactivate(true);
                            break;
                    }
                }
            }
            return 1;
        }

        int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return 1;
        }

        private void Selection_SelectionChanged(object sender, EventArgs e)
        {
            if (view.Selection.IsEmpty || IsActive)
                return;
            PushMark(view.Selection.Start.Position.Position, true);
        }

        private void UpdateSelection()
        {
            if (!IsActive)
                return;
            view.Selection.Select(new VirtualSnapshotPoint(currentMark.GetPoint(view.TextSnapshot)),
                new VirtualSnapshotPoint(view.GetCaretPosition()));
            view.Caret.EnsureVisible();
        }

        private void ClearSelection()
        {
            if (view.Selection.IsEmpty)
                return;
            view.Selection.Clear();
        }

        internal void SwapPointAndMark()
        {
            ITrackingPoint trackingPoint = activeMark;
            activeMark = currentMark = CreateTrackingPoint(view.GetCaretPosition());
            view.Caret.MoveTo(trackingPoint.GetPoint(view.TextSnapshot));
            IsActive = true;
            UpdateSelection();
        }

        internal void Activate()
        {
            IsActive = true;
            UpdateSelection();
        }

        private ITrackingPoint CreateTrackingPoint(int position)
        {
            return view.TextSnapshot.CreateTrackingPoint(position, PointTrackingMode.Negative);
        }

        internal void PushMark(bool activateSession = true)
        {
            PushMark(view.GetCaretPosition(), activateSession);
            if (!activateSession)
                return;
            UpdateSelection();
        }

        private void PushMark(int position, bool activateSession = true)
        {
            marks.Push(activeMark);
            activeMark = currentMark = CreateTrackingPoint(position);
            if (!activateSession)
                return;
            IsActive = true;
        }

        internal void PopMark()
        {
            if (marks.Count > 0)
            {
                if (currentMark != activeMark)
                    currentMark = activeMark;
                activeMark = marks.Pop();
            }
            else
                currentMark = activeMark = CreateTrackingPoint(0);
            view.Caret.MoveTo(currentMark.GetPoint(view.TextSnapshot));
            UpdateSelection();
        }

        internal void RemoveTopMark()
        {
            if (marks.Count <= 0)
                return;
            activeMark = marks.Pop();
        }

        internal static MarkSession GetSession(ITextView view)
        {
            if (view.Properties.ContainsProperty(typeof (MarkSession)))
                return view.Properties.GetProperty<MarkSession>(typeof (MarkSession));
            return null;
        }

        public void Deactivate(bool clearSelection = true)
        {
            if (clearSelection)
                ClearSelection();
            IsActive = false;
        }

        public override void PostprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Deactivate(false);
            base.PostprocessMouseLeftButtonDown(e);
        }
    }
}