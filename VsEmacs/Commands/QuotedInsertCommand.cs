namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.QuotedInsert)]
    internal class QuotedInsertCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            int? universalArgument1 = context.Manager.UniversalArgument;
            if ((universalArgument1.GetValueOrDefault() <= 0 ? 0 : (universalArgument1.HasValue ? 1 : 0)) != 0)
            {
                int? universalArgument2 = context.Manager.UniversalArgument;
                if ((universalArgument2.GetValueOrDefault() > (int) byte.MaxValue
                    ? 0
                    : (universalArgument2.HasValue ? 1 : 0)) != 0)
                {
                    context.EditorOperations.InsertText(((char) context.Manager.UniversalArgument.Value).ToString());
                    return;
                }
            }
            context.Manager.UpdateStatus("Use c-u to enter the ASCII decimal value first", false);
        }
    }
}