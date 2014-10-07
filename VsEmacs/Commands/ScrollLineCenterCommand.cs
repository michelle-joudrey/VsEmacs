using System;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.ScrollLineCenter)]
    internal class ScrollLineCenterCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            if (context.Manager.UniversalArgument.HasValue)
            {
                int num = context.Manager.UniversalArgument.Value;
                context.EditorOperations.ScrollLineTop();
                for (int index = 0; index < num; ++index)
                    context.EditorOperations.ScrollUpAndMoveCaretIfNecessary();
            }
            else
                context.EditorOperations.ScrollLineCenter();
        }

        internal override void ExecuteInverse(EmacsCommandContext context)
        {
            int num = Math.Abs(context.Manager.GetUniversalArgumentOrDefault(1));
            context.EditorOperations.ScrollLineBottom();
            for (int index = 0; index < num; ++index)
                context.EditorOperations.ScrollDownAndMoveCaretIfNecessary();
        }
    }
}