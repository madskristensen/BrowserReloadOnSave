using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace BrowserReloadOnSave
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(Options), "Web", Vsix.Name, 101, 102, true, new string[0], ProvidesLocalizedCategoryName = false)]
    [ProvideAutoLoad("{349C5852-65DF-11dA-9384-00065B846F21}", PackageAutoLoadFlags.BackgroundLoad)] // WAP
    [ProvideAutoLoad("{E24C65DC-7377-472b-9ABA-BC803B73C61A}", PackageAutoLoadFlags.BackgroundLoad)] // WebSite
    [ProvideAutoLoad("{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}", PackageAutoLoadFlags.BackgroundLoad)] // ProjectK
    [Guid(PackageGuids.guidBrowserReloadPackageString)]
    public sealed class VSPackage : Package
    {
        public static Options Options
        {
            get;
            private set;
        }

        protected override void Initialize()
        {
            Options = (Options)GetDialogPage(typeof(Options));

            Logger.Initialize(this, Vsix.Name);
            EnableReloadCommand.Initialize(this);
        }
    }
}
