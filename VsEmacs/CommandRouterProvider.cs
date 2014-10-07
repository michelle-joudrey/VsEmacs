using EnvDTE;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using IServiceProvider = System.IServiceProvider;

namespace VsEmacs
{
    [Export]
    internal class CommandRouterProvider
    {
        [Import] private ICompletionBroker CompletionBroker;
        [Import] private IVsEditorAdaptersFactoryService EditorAdapterFactoryService;
        [Import(typeof (SVsServiceProvider))] private IServiceProvider ServiceProvider;

        public CommandRouter GetCommandRouter(ITextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() =>
            {
                var ppNextCmdTarg = (IOleCommandTarget) null;
                IVsTextView viewAdapter = EditorAdapterFactoryService.GetViewAdapter(view);
                var service = ServiceProvider.GetService<DTE>();
                var commandRouter = new CommandRouter(view, viewAdapter as IOleCommandTarget, CompletionBroker, service);
                Marshal.ThrowExceptionForHR(viewAdapter.AddCommandFilter(commandRouter, out ppNextCmdTarg));
                commandRouter.Next = ppNextCmdTarg;
                return commandRouter;
            });
        }
    }
}