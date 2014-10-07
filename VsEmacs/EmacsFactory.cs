using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace VsEmacs
{
    [ContentType("text")]
    [Export(typeof (IVsTextViewCreationListener))]
    [TextViewRole("INTERACTIVE")]
    [Name("Emacs Emulation MouseProcessor")]
    [Order(Before = "default")]
    [Export(typeof (IMouseProcessorProvider))]
    internal class EmacsFactory : IVsTextViewCreationListener, IMouseProcessorProvider
    {
        [Import]
        private EmacsCommandsManager Manager { get; set; }

        [Import]
        private IVsEditorAdaptersFactoryService EditorAdaptersFactory { get; set; }

        [Import]
        private CommandRouterProvider CommandRouterProvider { get; set; }

        public IMouseProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            return Manager.GetOrCreateMarkSession(wpfTextView);
        }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IWpfTextView wpfTextView = EditorAdaptersFactory.GetWpfTextView(textViewAdapter);
            wpfTextView.Options.OptionChanged += OnOptionsChanged;
            if (wpfTextView.Roles.Contains("PRIMARYDOCUMENT"))
            {
                CommandRouter commandRouter = CommandRouterProvider.GetCommandRouter(wpfTextView);
                commandRouter.AddCommandTarget(new EmacsCommandsFilter(wpfTextView, Manager, commandRouter));
                commandRouter.AddCommandTarget(Manager.GetOrCreateMarkSession(wpfTextView));
                commandRouter.AddCommandTarget(Manager.GetOrCreateUniversalArgumentSession(wpfTextView));
                commandRouter.AddCommandTarget(Manager.GetOrCreateKillClipboardSession(wpfTextView));
            }
            else
            {
                var workAroundFilter = new InteractiveRoleWorkAroundFilter(wpfTextView, Manager);
                IOleCommandTarget ppNextCmdTarg;
                textViewAdapter.AddCommandFilter(workAroundFilter, out ppNextCmdTarg);
                workAroundFilter.NextCommandTarget = ppNextCmdTarg;
            }
        }

        private void OnOptionsChanged(object sender, EditorOptionChangedEventArgs e)
        {
            Manager.CheckEmacsVskSelected();
        }
    }
}