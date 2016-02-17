using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Web.BrowserLink;

namespace BrowserReloadOnSave
{
    public class ReloadExtension : BrowserLinkExtension, IDisposable
    {
        IEnumerable<string> _extensions = VSPackage.Options.FileExtensions.Split(';');
        IEnumerable<string> _ignorePatterns = VSPackage.Options.GetIgnorePatterns();
        List<BrowserLinkConnection> _connections = new List<BrowserLinkConnection>();
        bool _isDisposed;
        Project _project;

        public ReloadExtension(Project project)
        {
            _project = project;
            string folder = project.Properties.Item("FullPath").Value?.ToString();

            Watcher = new FileSystemWatcher(folder);
            Watcher.Changed += FileChanged;
            Watcher.IncludeSubdirectories = true;
            Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime;
            Watcher.EnableRaisingEvents = VSPackage.Options.EnableReload;

            VSPackage.Options.Saved += OptionsSaved;
        }

        public FileSystemWatcher Watcher { get; }

        public override void OnConnected(BrowserLinkConnection connection)
        {
            if (connection.Project == _project)
                _connections.Add(connection);

            base.OnConnected(connection);
        }

        public override void OnDisconnecting(BrowserLinkConnection connection)
        {
            if (_connections.Contains(connection))
                _connections.Remove(connection);

            base.OnDisconnecting(connection);
        }

        public void Reload(string extension)
        {
            Telemetry.TrackEvent("Saved ." + extension);
            Browsers.Clients(_connections.ToArray()).Invoke("reload");
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                VSPackage.Options.Saved -= OptionsSaved;
                Watcher.Dispose();
                _isDisposed = true;
            }
        }

        void FileChanged(object sender, FileSystemEventArgs e)
        {
            string file = e.FullPath.ToLowerInvariant();
            string ext = Path.GetExtension(file).TrimStart('.');

            if (_extensions.Contains(ext) && !_ignorePatterns.Any(p => file.Contains(p)))
            {
                // Only reload on CSS file changes if it's a ASP.NET Core project, due to a bug in Browser Link
                if (ext == "css" && !_project.Kind.Equals("{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}", StringComparison.OrdinalIgnoreCase))
                    return;

                var watcher = (FileSystemWatcher)sender;
                watcher.EnableRaisingEvents = false;
                Reload(ext);
                watcher.EnableRaisingEvents = true;
            }
        }

        void OptionsSaved(object sender, EventArgs e)
        {
            _extensions = VSPackage.Options.FileExtensions.Split(';');
            _ignorePatterns = VSPackage.Options.GetIgnorePatterns();
            Watcher.EnableRaisingEvents = VSPackage.Options.EnableReload;
            Telemetry.TrackEvent("Updated settings");
        }
    }
}
