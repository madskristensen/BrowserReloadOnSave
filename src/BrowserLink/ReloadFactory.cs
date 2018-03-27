using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Web.BrowserLink;

namespace BrowserReloadOnSave
{
    [Export(typeof(IBrowserLinkExtensionFactory))]
    public class ReloadFactory : IBrowserLinkExtensionFactory
    {
        public static Dictionary<Project, ReloadExtension> Extensions = new Dictionary<Project, ReloadExtension>();
        static SolutionEvents _solutionEvents;

        static ReloadFactory()
        {
            var dte = (DTE2)Package.GetGlobalService(typeof(DTE));
            _solutionEvents = dte.Events.SolutionEvents;
            _solutionEvents.AfterClosing += SolutionEvents_AfterClosing;
            _solutionEvents.ProjectRemoved += _solutionEvents_ProjectRemoved;
        }

        public BrowserLinkExtension CreateExtensionInstance(BrowserLinkConnection connection)
        {
            if (connection.Project == null)
                return null;

            // Create one extension per project
            if (!Extensions.ContainsKey(connection.Project))
            {
                var extension = new ReloadExtension(connection.Project);
                Extensions.Add(connection.Project, extension);
            }

            return Extensions[connection.Project];
        }

        public string GetScript()
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream("BrowserReloadOnSave.BrowserLink.Reload.js"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        static void _solutionEvents_ProjectRemoved(Project Project)
        {
            if (Extensions.ContainsKey(Project))
            {
                Extensions[Project].Dispose();
                Extensions.Remove(Project);
            }
        }

        static void SolutionEvents_AfterClosing()
        {
            foreach (Project project in Extensions.Keys)
            {
                Extensions[project].Dispose();
            }

            Extensions.Clear();
        }
    }
}
