﻿using Slicer.Source.Results;
using Slicer.Source.Tasks;

namespace Slicer.Source.Scanning
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using Slicer.Engine;
    using Slicer.Engine.Common;
    using Slicer.Engine.Common.DataTypes;
    using Slicer.Engine.Scanning.Scanners;
    using Slicer.Engine.Scanning.Snapshots;
    using Slicer.Source.Results;
    using Slicer.Source.Tasks;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Collect values for the current snapshot, or a new one if none exists. The values are then assigned to a new snapshot.
    /// </summary>
    public class ValueCollectorViewModel : ViewModelBase
    {
        /// <summary>
        /// Singleton instance of the <see cref="ValueCollectorViewModel" /> class.
        /// </summary>
        private static Lazy<ValueCollectorViewModel> valueCollectorViewModelInstance = new Lazy<ValueCollectorViewModel>(
                () => { return new ValueCollectorViewModel(); },
                LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueCollectorViewModel" /> class.
        /// </summary>
        public ValueCollectorViewModel()
        {
            this.CollectValuesCommand = new RelayCommand(() => Task.Run(() => this.CollectValues()), () => true);
        }

        /// <summary>
        /// Gets the command to collect values.
        /// </summary>
        public ICommand CollectValuesCommand { get; private set; }

        /// <summary>
        /// Gets a singleton instance of the <see cref="ValueCollectorViewModel"/> class.
        /// </summary>
        /// <returns>A singleton instance of the class.</returns>
        public static ValueCollectorViewModel GetInstance()
        {
            return valueCollectorViewModelInstance.Value;
        }

        /// <summary>
        /// Begins the value collection.
        /// </summary>
        private void CollectValues()
        {
            DataTypeBase dataType = ScanResultsViewModel.GetInstance().ActiveType;

            TrackableTask<Snapshot> valueCollectTask = ValueCollector.CollectValues(
                SessionManager.Session?.OpenedProcess,
                SessionManager.Session.SnapshotManager.GetActiveSnapshotCreateIfNone(SessionManager.Session.OpenedProcess),
                TrackableTask.UniversalIdentifier
            );

            TaskTrackerViewModel.GetInstance().TrackTask(valueCollectTask);
            SessionManager.Session.SnapshotManager.SaveSnapshot(valueCollectTask.Result);
        }
    }
    //// End class
}
//// End namespace