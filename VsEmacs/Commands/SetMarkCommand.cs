using System.ComponentModel.Composition;

namespace VsEmacs.Commands
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [EmacsCommand(EmacsCommandID.SetMark)]
    internal class SetMarkCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            int? universalArgument1 = context.UniversalArgument;
            if ((universalArgument1.GetValueOrDefault() != 4 ? 0 : (universalArgument1.HasValue ? 1 : 0)) != 0)
            {
                context.MarkSession.PopMark();
            }
            else
            {
                int? universalArgument2 = context.UniversalArgument;
                if ((universalArgument2.GetValueOrDefault() != 16 ? 0 : (universalArgument2.HasValue ? 1 : 0)) != 0)
                    context.MarkSession.RemoveTopMark();
                else
                    context.MarkSession.PushMark(true);
            }
        }
    }
}