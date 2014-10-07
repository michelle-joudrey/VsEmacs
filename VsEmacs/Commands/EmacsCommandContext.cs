using System.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;

namespace VsEmacs.Commands
{
    internal class EmacsCommandContext
    {
        internal EmacsCommandContext(EmacsCommandsManager manager,
            ITextStructureNavigatorSelectorService textStructureNavigatorSelectorService,
            IEditorOperations editorOperations, ITextView view, CommandRouter commandRouter)
        {
            Manager = manager;
            EditorOperations = editorOperations;
            TextView = view;
            CommandRouter = commandRouter;
            Clipboard = new StringBuilder();
            TextStructureNavigator = textStructureNavigatorSelectorService.GetTextStructureNavigator(view.TextBuffer);
            MarkSession = MarkSession.GetSession(view);
        }

        internal ITextStructureNavigator TextStructureNavigator { get; private set; }

        internal IEditorOperations EditorOperations { get; private set; }

        internal ITextView TextView { get; private set; }

        internal ITextBuffer TextBuffer
        {
            get { return TextView.TextBuffer; }
        }

        internal EmacsCommandsManager Manager { get; private set; }

        internal CommandRouter CommandRouter { get; private set; }

        internal int? UniversalArgument
        {
            get { return Manager.UniversalArgument; }
        }

        internal StringBuilder Clipboard { get; private set; }

        internal MarkSession MarkSession { get; private set; }
    }
}