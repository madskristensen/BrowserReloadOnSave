using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace BrowserReloadOnSave
{
    sealed class EnableReloadCommand
    {
        readonly Package _package;

        EnableReloadCommand(Package package)
        {
            _package = package;

            var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var id = new CommandID(PackageGuids.guidBrowserReloadCmdSet, PackageIds.EnableReloadCommandId);
                var cmd = new OleMenuCommand(Execute, id);
                cmd.BeforeQueryStatus += BeforeQueryStatus;
                commandService.AddCommand(cmd);
            }
        }

        public static EnableReloadCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new EnableReloadCommand(package);
        }

        IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        void BeforeQueryStatus(object sender, EventArgs e)
        {
            var button = (OleMenuCommand)sender;

            button.Checked = VSPackage.Options.EnableReload;
        }

        void Execute(object sender, EventArgs e)
        {
            var button = (OleMenuCommand)sender;

            VSPackage.Options.EnableReload = !button.Checked;
            VSPackage.Options.SaveSettingsToStorage();
        }
    }
}
