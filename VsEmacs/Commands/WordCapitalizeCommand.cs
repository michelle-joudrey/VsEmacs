using Microsoft.VisualStudio.Text;

namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.WordCapitalize, CanBeRepeated = true, UndoName = "Change case")]
    internal class WordCapitalizeCommand : EmacsCommand
    {
        internal override void Execute(EmacsCommandContext context)
        {
            SnapshotSpan? nextWord = context.TextStructureNavigator.GetNextWord(context.TextView);
            if (!nextWord.HasValue)
                return;
            if (context.TextView.GetCaretPosition().Position < nextWord.Value.Start)
                context.EditorOperations.MoveCaret(nextWord.Value.Start);
            context.EditorOperations.MakeUppercase();
            for (int position = context.TextView.GetCaretPosition().Position;
                position < (int) nextWord.Value.End;
                ++position)
                context.EditorOperations.MakeLowercase();
        }

        internal override void ExecuteInverse(EmacsCommandContext context)
        {
            SnapshotSpan? previousWord = context.TextStructureNavigator.GetPreviousWord(context.TextView);
            if (!previousWord.HasValue)
                return;
            if (context.TextView.GetCaretPosition().Position > previousWord.Value.End)
                context.EditorOperations.MoveCaret(previousWord.Value.End);
            int position1 = previousWord.Value.Start.Position;
            for (int position2 = context.TextView.GetCaretPosition().Position - 1; position2 > position1; --position2)
            {
                context.EditorOperations.MoveCaret(position2);
                context.EditorOperations.MakeLowercase();
            }
            context.EditorOperations.MoveCaret(position1);
            context.EditorOperations.MakeUppercase();
            context.EditorOperations.MoveCaret(position1);
        }
    }
}