﻿namespace Slicer.Cli
{
    using Engine.Common.Logging;
    using System;

    public class LogListener : ILoggerObserver
    {
        public void OnLogEvent(LogLevel logLevel, String message, String innerMessage)
        {
            Console.WriteLine(message);
        }
    }
    //// End class
}
//// End namespace
