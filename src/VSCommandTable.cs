namespace BrowserReloadOnSave
{
    using System;
    
    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string guidBrowserReloadPackageString = "2d8aa02a-8810-421f-97b9-86efc573fea3";
        public const string guidBrowserReloadCmdSetString = "44f3346d-7059-4428-9d81-2f16be71e28e";
        public const string guidBrowserLinkCmdSetString = "30947ebe-9147-45f9-96cf-401bfc671a82";
        public static Guid guidBrowserReloadPackage = new Guid(guidBrowserReloadPackageString);
        public static Guid guidBrowserReloadCmdSet = new Guid(guidBrowserReloadCmdSetString);
        public static Guid guidBrowserLinkCmdSet = new Guid(guidBrowserLinkCmdSetString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int EnableReloadCommandId = 0x0100;
        public const int IDG_BROWSERLINK_COMMANDS = 0x2001;
    }
}
