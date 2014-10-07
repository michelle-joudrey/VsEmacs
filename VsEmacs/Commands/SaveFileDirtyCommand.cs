using Microsoft.VisualStudio.Shell.Interop;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.FileSaveDirty)]
    internal class SaveFileDirtyCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            IVsRunningDocumentTable service =
                context.Manager.ServiceProvider.GetService<SVsRunningDocumentTable, IVsRunningDocumentTable>();
            if (service == null)
                return;
            service.SaveDocuments(1U, null, 4294967294U, 0U);
        }
    }
}