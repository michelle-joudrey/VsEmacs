using EnvDTE;
namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.CloseOtherWindow)]
    internal class CloseOtherWindowCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            var service = context.Manager.ServiceProvider.GetService<DTE>();
            if (service.ActiveDocument == null || service.ActiveDocument.ActiveWindow == null)
                return;
            var textWindow = service.ActiveDocument.ActiveWindow.Object as TextWindow;
            if (textWindow == null || textWindow.Panes.Count != 2)
                return;
            context.CommandRouter.ExecuteDTECommand("Window.Split");
        }
    }
}