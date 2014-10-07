using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.ExtendedCommand)]
    internal class ExtendedCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            IVsUIShell service = context.Manager.ServiceProvider.GetService<SVsUIShell, IVsUIShell>();
            if (service == null)
                return;
            Guid guid = typeof (VSConstants.VSStd97CmdID).GUID;
            object pvaIn = 0;
            service.PostExecCommand(ref guid, 429U, 0U, ref pvaIn);
        }
    }
}