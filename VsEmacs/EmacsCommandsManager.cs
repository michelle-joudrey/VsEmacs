using EnvDTE;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.Win32;
using VsEmacs.Commands;

namespace VsEmacs
{
    [Export]
    internal class EmacsCommandsManager : IPartImportsSatisfiedNotification
    {
        internal const string EmacsVskFile = "VsEmacs.vsk";
        private StringBuilder changes;

        public EmacsCommandsManager()
        {
            ClipboardRing = new List<string>();
            ClipboardRingIndex = -1;
        }

        [Import(typeof (SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; private set; }

        [ImportMany]
        private IEnumerable<Lazy<EmacsCommand, IEmacsCommandMetadata>> Commands { get; set; }

        [Import]
        private IEditorOperationsFactoryService EditorOperationsFactoryService { get; set; }

        [Import]
        public ITextStructureNavigatorSelectorService TextStructureNavigatorSelectorService { get; set; }

        [Import]
        public ITextUndoHistoryRegistry TextUndoHistoryRegistry { get; set; }

        [Import]
        public ISmartIndentationService SmartIndentationService { get; set; }

        [Import]
        private CommandRouterProvider CommandRouterProvider { get; set; }

        public IComponentModel ComponentModel { get; private set; }

        public bool IsEnabled { get; set; }

        internal int? UniversalArgument { get; set; }

        internal bool IsEmacsVskInstalled
        {
            get { return File.Exists(Path.Combine(EmacsInstallationPath, "VsEmacs.vsk")); }
        }

        internal string EmacsInstallationPath
        {
            get { return GetVsInstallPath(); }
        }

        public List<string> ClipboardRing { get; set; }

        public int ClipboardRingIndex { get; set; }

        public IEmacsCommandMetadata LastExecutedCommand { get; set; }

        public bool AfterSearch { get; set; }

        public void OnImportsSatisfied()
        {
            ComponentModel = ServiceProvider.GetService<SComponentModel, IComponentModel>();
        }

        internal int GetUniversalArgumentOrDefault(int defaultValue = 0)
        {
            if (!UniversalArgument.HasValue)
                return defaultValue;
            return UniversalArgument.Value;
        }

        public void Execute(ITextView view, EmacsCommandID commandId, bool evaluateUniversalArgument = true)
        {
            IEmacsCommandMetadata commandMetadata = GetCommandMetadata(commandId);
            if (commandMetadata == null)
                return;
            Execute(view, commandMetadata, evaluateUniversalArgument);
        }

        public void Execute(ITextView view, IEmacsCommandMetadata metadata, bool evaluateUniversalArgument = true)
        {
            try
            {
                changes = new StringBuilder();
                view.TextBuffer.Changed += OnTextBufferChanged;
                EmacsCommand command = CreateCommand(metadata);
                EmacsCommand emacsCommand = null;
                IEmacsCommandMetadata metadata1 = null;
                var context = new EmacsCommandContext(this, TextStructureNavigatorSelectorService,
                    EditorOperationsFactoryService.GetEditorOperations(view), view,
                    CommandRouterProvider.GetCommandRouter(view));
                if (ClipboardRing.Count == 0 ||  ClipboardRing.Last() != Clipboard.GetText())
                {
                    ClipboardRing.Add(Clipboard.GetText());
                }
                if (command == null)
                    return;
                ITextUndoHistory history = TextUndoHistoryRegistry.GetHistory(context.TextBuffer);
                using (ITextUndoTransaction transaction = CreateTransaction(metadata, history))
                {
                    int num = 1;
                    bool flag = false;
                    if (evaluateUniversalArgument)
                    {
                        flag = GetUniversalArgumentOrDefault(0) < 0;
                        if (flag)
                        {
                            metadata1 = GetCommandMetadata(metadata.InverseCommand);
                            emacsCommand = CreateCommand(metadata1);
                        }
                        if (metadata.CanBeRepeated)
                            num = Math.Abs(GetUniversalArgumentOrDefault(1));
                    }
                    for (; num > 0; --num)
                    {
                        if (flag)
                        {
                            if (emacsCommand != null)
                                emacsCommand.Execute(context);
                            else
                                command.ExecuteInverse(context);
                        }
                        else
                            command.Execute(context);
                    }
                    if (transaction != null)
                        transaction.Complete();
                    if (context.Clipboard.Length > 0)
                    {
                        ClipboardRing.Add(context.Clipboard.ToString());
                        Clipboard.SetText(context.Clipboard.ToString());
                    }
                    else if (changes.Length > 0 && metadata.CopyDeletedTextToTheClipboard)
                    {
                        ClipboardRing.Add(changes.ToString());
                        Clipboard.SetText(changes.ToString());
                    }
                    LastExecutedCommand = flag ? metadata1 : metadata;
                }
            }
            catch (NoOperationException ex)
            {
            }
            finally
            {
                view.TextBuffer.Changed -= OnTextBufferChanged;
            }
        }

        private static ITextUndoTransaction CreateTransaction(IEmacsCommandMetadata metadata, ITextUndoHistory history)
        {
            if (string.IsNullOrEmpty(metadata.UndoName))
                return null;
            return history.CreateTransaction(metadata.UndoName);
        }

        private void OnTextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            e.Changes.ToList().ForEach(change => changes.Append(change.OldText));
        }

        private EmacsCommand CreateCommand(IEmacsCommandMetadata metadata)
        {
            if (metadata == null)
                return null;
            Lazy<EmacsCommand, IEmacsCommandMetadata> lazy = Commands.FirstOrDefault(export =>
            {
                if (export.Metadata.Command == metadata.Command)
                    return export.Metadata.CommandGroup == metadata.CommandGroup;
                return false;
            });
            if (lazy != null)
                return lazy.Value;
            return null;
        }

        internal IEmacsCommandMetadata GetCommandMetadata(EmacsCommandID command)
        {
            return GetCommandMetadata((int) command, typeof (EmacsCommandID).GUID);
        }

        internal IEmacsCommandMetadata GetCommandMetadata(int commandId, Guid commandGroup)
        {
            return Commands.Select(lazy => lazy.Metadata).FirstOrDefault(metadata =>
            {
                if (metadata.Command == commandId)
                    return new Guid(metadata.CommandGroup) == commandGroup;
                return false;
            });
        }

        internal void ClearStatus()
        {
            UpdateStatus(string.Empty, false);
        }

        internal void UpdateStatus(string text, bool append = false)
        {
            var service = ServiceProvider.GetService<DTE>();
            if (service == null || service.StatusBar == null)
                return;
            if (append)
            {
                EnvDTE.StatusBar statusBar = service.StatusBar;
                string str = statusBar.Text + text;
                statusBar.Text = str;
            }
            else if (string.IsNullOrEmpty(text))
                service.StatusBar.Clear();
            else
                service.StatusBar.Text = text;
        }

        private string GetVsInstallPath()
        {
            ILocalRegistry2 service = ServiceProvider.GetService<SLocalRegistry, ILocalRegistry2>();
            var pbstrRoot = (string) null;
            service.GetLocalRegistryRoot(out pbstrRoot);
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(pbstrRoot))
                return Path.GetDirectoryName(registryKey.GetValue("InstallDir") as string);
        }

        internal void CheckEmacsVskSelected()
        {
            try
            {
                if (!IsEmacsVskInstalled)
                    return;
                ILocalRegistry2 service = ServiceProvider.GetService<SLocalRegistry, ILocalRegistry2>();
                var pbstrRoot = (string) null;
                service.GetLocalRegistryRoot(out pbstrRoot);
                using (RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey(pbstrRoot))
                {
                    if (registryKey1 == null)
                        return;
                    using (RegistryKey registryKey2 = registryKey1.OpenSubKey("Keyboard"))
                    {
                        if (registryKey2 == null)
                            return;
                        var path = registryKey2.GetValue("SchemeName") as string;
                        IsEnabled = !string.IsNullOrEmpty(path) &&
                                    string.Equals("VsEmacs.vsk", Path.GetFileName(path),
                                        StringComparison.InvariantCultureIgnoreCase);
                    }
                }
            }
            catch
            {
                IsEnabled = false;
            }
        }

        public KillwordClipboardSession GetOrCreateKillClipboardSession(ITextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new KillwordClipboardSession(view, this));
        }

        public MarkSession GetOrCreateMarkSession(ITextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new MarkSession(view, this));
        }

        public UniversalArgumentSession GetOrCreateUniversalArgumentSession(ITextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new UniversalArgumentSession(view, this));
        }
    }
}