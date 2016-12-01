using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace BrowserReloadOnSave
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(Options), "Web", Vsix.Name, 101, 102, true, new string[0], ProvidesLocalizedCategoryName = false)]
    [ProvideAutoLoad(ActivationContextGuid)]
    [ProvideUIContextRule(ActivationContextGuid, "Load Package",
        "WAP | WebSite | ProjectK | DotNetCoreWeb",
        new string[] {
            "WAP",
            "WebSite",
            "ProjectK",
            "DotNetCoreWeb"
        },
        new string[] {
            "SolutionHasProjectFlavor:{349C5851-65DF-11DA-9384-00065B846F21}",
            "SolutionHasProjectFlavor:{E24C65DC-7377-472B-9ABA-BC803B73C61A}",
            "SolutionHasProjectFlavor:{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}",
            "SolutionHasProjectCapability:DotNetCoreWeb"
        })]
    [Guid(PackageGuids.guidBrowserReloadPackageString)]
    public sealed class VSPackage : AsyncPackage
    {
        private const string ActivationContextGuid = "{4b6c8d76-4918-45aa-9b26-8f246c1773aa}";

        public static Options Options
        {
            get;
            private set;
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Logger.Initialize(Vsix.Name);

            Options = (Options)GetDialogPage(typeof(Options));
            var commandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                EnableReloadCommand.Initialize(this, commandService);
            }
        }
    }
}
