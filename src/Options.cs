using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace BrowserReloadOnSave
{
    public class Options : DialogPage
    {
        const string fileExtensions = "css;js;html;htm;cshtml;vbhtml;aspx;ascx;master";
        const string ignoreList = @"\node_modules\;\bower_components\;\typings\;\lib\;\vendor\";

        [Category("General")]
        [DisplayName("Enable reload on save")]
        [Description("When enabled, every time a file is saved the connected browsers will reload.")]
        [DefaultValue(true)]
        public bool EnableReload { get; set; } = true;

        [Category("Filter")]
        [DisplayName("File extensions")]
        [Description("A semicolon-separated list of file extensions to listen to.")]
        [DefaultValue(fileExtensions)]
        public string FileExtensions { get; set; } = fileExtensions;

        [Category("Filter")]
        [DisplayName("Ignore patterns")]
        [Description("A semicolon-separated list of strings. Any file containing one of the strings in the path will be ignored.")]
        [DefaultValue(ignoreList)]
        public string IgnorePatterns { get; set; } = ignoreList;

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            if (Saved != null)
                Saved(this, EventArgs.Empty);
        }

        public IEnumerable<string> GetIgnorePatterns()
        {
            var raw = IgnorePatterns.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pattern in raw)
            {
                yield return pattern;
            }
        }

        public event EventHandler<EventArgs> Saved;
    }
}
