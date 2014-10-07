using System;

namespace VsEmacs.Commands
{
    internal abstract class EmacsCommand
    {
        internal abstract void Execute(EmacsCommandContext context);

        internal virtual void ExecuteInverse(EmacsCommandContext context)
        {
            throw new NotImplementedException();
        }
    }
}