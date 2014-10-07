using System.ComponentModel.Composition;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.PopMark)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PopMarkCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            context.MarkSession.PopMark();
        }
    }
}