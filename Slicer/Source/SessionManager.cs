﻿namespace Slicer.Source
{
    using Slicer.Engine;
    using Slicer.Engine.Projects;

    public static class SessionManager
    {
        private static Session session = new Session(null);

        public static Session Session
        {
            get
            {
                return SessionManager.session;
            }

            private set
            {
                SessionManager.session = value;
            }
        }

        public static Project Project { get; set; }
    }
    //// End class
}
//// End namespace
