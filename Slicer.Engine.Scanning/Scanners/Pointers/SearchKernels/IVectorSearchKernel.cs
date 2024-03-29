﻿using Slicer.Engine.Scanning.Snapshots;

namespace Slicer.Engine.Scanning.Scanners.Pointers.SearchKernels
{
    using Scanning.Snapshots;
    using System;
    using System.Numerics;

    /// <summary>
    /// Defines an interface for an object that can search for pointers that point within a specified offset of a given set of snapshot regions.
    /// </summary>
    internal interface IVectorSearchKernel
    {
        Func<Vector<Byte>> GetSearchKernel(SnapshotElementVectorComparer snapshotElementVectorComparer);
    }
    //// End interface
}
//// End namespace