namespace VsEmacs.Commands
{
    public interface IEmacsCommandMetadata
    {
        string CommandGroup { get; }

        int Command { get; }

        EmacsCommandID InverseCommand { get; }

        bool CanBeRepeated { get; }

        bool CopyDeletedTextToTheClipboard { get; }

        string UndoName { get; }
    }
}