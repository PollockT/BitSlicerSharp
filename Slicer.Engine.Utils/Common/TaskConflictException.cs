using System;

namespace Slicer.Engine.Common.Common
{
    public class TaskConflictException : Exception
    {
        public TaskConflictException() : base()
        {
        }

        public TaskConflictException(String message) : base(message)
        {
        }

        public TaskConflictException(String message, Exception inner) : base(message, inner)
        {
        }
    }
    //// End class
}
//// End namespace