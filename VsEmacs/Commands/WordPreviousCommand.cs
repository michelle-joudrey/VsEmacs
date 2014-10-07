using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.WordPrevious, CanBeRepeated = true, InverseCommand = EmacsCommandID.WordNext)]
    internal class WordPreviousCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            SnapshotSpan? previousWord = context.TextStructureNavigator.GetPreviousWord(context.TextView);
            if (!previousWord.HasValue)
                return;
            context.EditorOperations.MoveCaret(previousWord.Value.Start);
        }
    }
}