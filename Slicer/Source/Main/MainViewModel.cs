﻿using Slicer.Source.Docking;
using Slicer.Source.Output;
using Slicer.Source.Updater;

namespace Slicer.Source.Main
{
    using Slicer.Engine.Common.Logging;
    using Slicer.Engine.Common.OS;
    using Slicer.Source.Docking;
    using Slicer.Source.Output;
    using Slicer.Source.Updater;
    using System;
    using System.Numerics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Main view model.
    /// </summary>
    public class MainViewModel : WindowHostViewModel
    {
        /// <summary>
        /// Singleton instance of the <see cref="MainViewModel" /> class
        /// </summary>
        private static Lazy<MainViewModel> mainViewModelInstance = new Lazy<MainViewModel>(
                () => { return new MainViewModel(); },
                LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Prevents a default instance of the <see cref="MainViewModel" /> class from being created.
        /// </summary>
        private MainViewModel() : base()
        {
            // Attach the logger view model to the engine's output
            Logger.Subscribe(OutputViewModel.GetInstance());

            ApplicationUpdater.UpdateApp();

            Slicer.Engine.Projects.Compiler.Compile(true);

            if (Vectors.HasVectorSupport)
            {
                Logger.Log(LogLevel.Info, "Hardware acceleration enabled (vector size: " + Vector<Byte>.Count + ")");
            }

            Logger.Log(LogLevel.Info, "Slicer started");

            // this.DisplayChangeLogCommand = new RelayCommand(() => ChangeLogViewModel.GetInstance().DisplayChangeLog(new Content.ChangeLog().TransformText()), () => true);
        }

        /// <summary>
        /// Gets the command to open the change log.
        /// </summary>
        public ICommand DisplayChangeLogCommand { get; private set; }

        /// <summary>
        /// Default layout file for browsing cheats.
        /// </summary>
        protected override String DefaultLayoutResource { get { return "DefaultLayout.xml"; } }

        /// <summary>
        /// The save file for the docking layout.
        /// </summary>
        protected override String LayoutSaveFile { get { return "Layout.xml"; } }

        /// <summary>
        /// Gets the singleton instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <returns>The singleton instance of the <see cref="MainViewModel" /> class.</returns>
        public static MainViewModel GetInstance()
        {
            return mainViewModelInstance.Value;
        }

        /// <summary>
        /// Closes the main window.
        /// </summary>
        /// <param name="window">The window to close.</param>
        protected override void Close(Window window)
        {
            // SolutionExplorerViewModel.GetInstance().DisableAllProjectItems();

            base.Close(window);
        }
    }
    //// End class
}
//// End namesapce