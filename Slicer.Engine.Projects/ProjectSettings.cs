using Slicer.Engine.Projects.Properties;

namespace Slicer.Engine.Projects
{
    using Projects.Properties;
    using System;
    using System.IO;

    public static class ProjectSettings
    {
        public static String ProjectRoot
        {
            get
            {
                if (Settings.Default.ProjectRoot.IsEmpty())
                {
                    ProjectSettings.ProjectRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slicer");
                }

                return Settings.Default.ProjectRoot;
            }

            set
            {
                Settings.Default.ProjectRoot = value;
            }
        }
    }
    //// End class
}
//// End namespace
