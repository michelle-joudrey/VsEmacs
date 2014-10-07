using EnvDTE;
namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.SplitVertical)]
    internal class SplitVerticalCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            var service = context.Manager.ServiceProvider.GetService<DTE>();
            if (service.ActiveDocument == null || service.ActiveDocument.ActiveWindow == null)
                return;
            var textWindow = service.ActiveDocument.ActiveWindow.Object as TextWindow;
            if (textWindow == null || textWindow.Panes.Count != 1)
                return;
            context.CommandRouter.ExecuteDTECommand("Window.Split");
        }
    }
}