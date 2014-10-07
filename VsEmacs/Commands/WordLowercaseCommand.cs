namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.WordLowercase, CanBeRepeated = true, UndoName = "Change case")]
    internal class WordLowercaseCommand : WordCasingCommandBase
    {
        internal override string TransformText(string text)
        {
            return text.ToLower();
        }
    }
}