namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.UniversalArgument)]
    internal class UniversalArgumentCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            UniversalArgumentSession.GetSession(context.TextView).Start();
        }
    }
}