﻿namespace Slicer.Engine.Scanning.Snapshots
{
    using Squalr.Engine.Common.DataStructures;
    using Squalr.Engine.Common.DataTypes;
    using Scanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class SnapshotManager
    {
        /// <summary>
        /// The size limit for snapshots to be saved in the snapshot history (256MB). TODO: Make this a setting.
        /// </summary>
        private const UInt64 SizeLimit = 1UL << 28;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotManager" /> class.
        /// </summary>
        public SnapshotManager()
        {
            this.AccessLock = new Object();
            this.Snapshots = new FullyObservableCollection<Snapshot>();
            this.DeletedSnapshots = new Stack<Snapshot>();
        }

        /// <summary>
        /// Gets the snapshots being managed.
        /// </summary>
        public FullyObservableCollection<Snapshot> Snapshots { get; private set; }

        /// <summary>
        /// Gets the deleted snapshots for the capability of redoing after undo.
        /// </summary>
        public Stack<Snapshot> DeletedSnapshots { get; private set; }

        /// <summary>
        /// Gets or sets a lock to ensure multiple entities do not try and update the snapshot list at the same time.
        /// </summary>
        private Object AccessLock { get; set; }

        public delegate void OnSnapshotsUpdated(SnapshotManager snapshotManager);

        public event OnSnapshotsUpdated OnSnapshotsUpdatedEvent;

        /// <summary>
        /// Returns the memory regions associated with the current snapshot. If none exist, a query will be done. Will not read any memory.
        /// </summary>
        /// <returns>The current active snapshot of memory in the target process.</returns>
        public Snapshot GetActiveSnapshotCreateIfNone(Process process)
        {
            lock (this.AccessLock)
            {
                if (this.Snapshots.Count == 0 || this.Snapshots.Peek() == null || this.Snapshots.Peek().ElementCount == 0)
                {
                    Snapshot snapshot = SnapshotQuery.GetSnapshot(process, SnapshotQuery.SnapshotRetrievalMode.FromSettings);
                    snapshot.Align(ScanSettings.Alignment);

                    return snapshot;
                }

                // Return the snapshot
                return this.Snapshots.Peek();
            }
        }

        /// <summary>
        /// Gets the current active snapshot.
        /// </summary>
        /// <returns>The current active snapshot of memory in the target process.</returns>
        public Snapshot GetActiveSnapshot()
        {
            lock (this.AccessLock)
            {
                // Take a snapshot if there are none, or the current one is empty
                if (this.Snapshots.Count == 0 || this.Snapshots.Peek() == null || this.Snapshots.Peek().ElementCount == 0)
                {
                    return null;
                }

                // Return the snapshot
                return this.Snapshots.Peek();
            }
        }

        /// <summary>
        /// Reverses an undo action.
        /// </summary>
        public void RedoSnapshot()
        {
            lock (this.AccessLock)
            {
                if (this.DeletedSnapshots.Count == 0)
                {
                    return;
                }

                this.Snapshots.Push(this.DeletedSnapshots.Pop());
                this.OnSnapshotsUpdatedEvent.Invoke(this);
            }
        }

        /// <summary>
        /// Undoes the current active snapshot, reverting to the previous snapshot.
        /// </summary>
        public void UndoSnapshot()
        {
            lock (this.AccessLock)
            {
                if (this.Snapshots.Count == 0)
                {
                    return;
                }

                this.DeletedSnapshots.Push(this.Snapshots.Pop());

                if (this.DeletedSnapshots.Peek() == null)
                {
                    this.DeletedSnapshots.Pop();
                }

                this.OnSnapshotsUpdatedEvent.Invoke(this);
            }
        }

        /// <summary>
        /// Clears all snapshot records.
        /// </summary>
        public void ClearSnapshots()
        {
            lock (this.AccessLock)
            {
                this.Snapshots.Clear();
                this.DeletedSnapshots.Clear();
                this.OnSnapshotsUpdatedEvent.Invoke(this);

                // There can be multiple GB of deleted snapshots, so run the garbage collector ASAP for a performance boost.
                Task.Run(() => GC.Collect());
            }
        }

        /// <summary>
        /// Saves a new snapshot, which will become the current active snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot to save.</param>
        public void SaveSnapshot(Snapshot snapshot)
        {
            lock (this.AccessLock)
            {
                // Remove null snapshot if exists
                if (this.Snapshots.Count != 0 && this.Snapshots.Peek() == null)
                {
                    this.Snapshots.Pop();
                }

                // Do not keep large snapshots in the undo history
                if (this.Snapshots.Count != 0 && this.Snapshots.Peek() != null && this.Snapshots.Peek().ByteCount > SnapshotManager.SizeLimit)
                {
                    this.Snapshots.Pop();
                }

                this.Snapshots.Push(snapshot);
                this.DeletedSnapshots.Clear();
                this.OnSnapshotsUpdatedEvent.Invoke(this);
            }
        }
    }
    //// End class
}
//// End namespace