namespace VsEmacs.Commands
{
    [EmacsCommand(EmacsCommandID.WordUppercase, CanBeRepeated = true, UndoName = "Change case")]
    internal class WordUppercaseCommand : WordCasingCommandBase
    {
        internal override string TransformText(string text)
        {
            return text.ToUpper();
        }
    }
}