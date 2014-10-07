using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

namespace VsEmacs
{
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad("{F1536EF8-92EC-443C-9ED7-FDADF150DA82}")]
    public sealed class VsEmacs : Package
    {
        private const string InstallFilename = "EmacsSetup.bat";

        private bool IsAdministrator
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            var service = this.GetService<SComponentModel, IComponentModel>().GetService<EmacsCommandsManager>();
            try
            {
                if (!service.IsEmacsVskInstalled)
                {
                    string installPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "VsEmacs.vsk");
                    var confirmationDialog = new CopyFileConfirmationDialog();
                    confirmationDialog.StartPosition = FormStartPosition.CenterScreen;
                    if (!IsAdministrator)
                    {
                        if (confirmationDialog.ShowDialog() != DialogResult.OK)
                            goto label_6;
                    }
                    CopyVskUsingXCopy(installPath, service);
                }
            }
            catch (Exception ex)
            {
            }
            label_6:
            service.CheckEmacsVskSelected();
        }

        private void CopyVskUsingXCopy(string installPath, EmacsCommandsManager manager)
        {
            var process = new Process();
            process.StartInfo.FileName = "xcopy.exe";
            process.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\"", installPath, manager.EmacsInstallationPath);
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major > 5)
                process.StartInfo.Verb = "runas";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
        }
    }
}