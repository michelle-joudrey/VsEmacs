using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio;

namespace VsEmacs.Commands
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EmacsCommandAttribute : ExportAttribute, IEmacsCommandMetadata
    {
        public EmacsCommandAttribute(EmacsCommandID command)
            : this((int) command, typeof (EmacsCommandID).GUID)
        {
        }

        public EmacsCommandAttribute(VSConstants.VSStd97CmdID command)
            : this((int) command, typeof (VSConstants.VSStd97CmdID).GUID)
        {
        }

        public EmacsCommandAttribute(VSConstants.VSStd2KCmdID command)
            : this((int) command, typeof (VSConstants.VSStd2KCmdID).GUID)
        {
        }

        private EmacsCommandAttribute(int command, Guid commandGroup)
            : base(typeof (EmacsCommand))
        {
            Command = command;
            CommandGroup = commandGroup.ToString();
            CanBeRepeated = false;
        }

        public string CommandGroup { get; set; }

        public bool CopyDeletedTextToTheClipboard { get; set; }

        public int Command { get; private set; }

        public EmacsCommandID InverseCommand { get; set; }

        public bool CanBeRepeated { get; set; }

        public string UndoName { get; set; }
    }
}