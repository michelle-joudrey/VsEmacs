using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.WordNext, CanBeRepeated = true, InverseCommand = EmacsCommandID.WordPrevious)]
    internal class WordNextCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            SnapshotSpan? nextWord = context.TextStructureNavigator.GetNextWord(context.TextView);
            if (!nextWord.HasValue)
                return;
            context.EditorOperations.MoveCaret(nextWord.Value.End);
        }
    }
}