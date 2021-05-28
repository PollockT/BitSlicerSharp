namespace Slicer.Cli
{
    using Engine.Common.Logging;
    using System;

    /// <summary>
    /// Slicer.Cli is a command line version of Slicer. This project is useful for ensuring that engine code is decoupled from view code.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Slicer.Cli entry point.
        /// </summary>
        /// <param name="args">Unused args.</param>
        static void Main(String[] args)
        {
            PrefixedWriter prefixedWriter = new PrefixedWriter();
            Console.SetOut(prefixedWriter);
            Console.SetIn(new PrefixedReader(prefixedWriter));

            CommandDispatcher commandDispatcher = new CommandDispatcher();
            LogListener logListener = new LogListener();

            Logger.Subscribe(logListener);

            while (true)
            {
                Console.Write("");
                String input = Console.ReadLine();

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("close", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                commandDispatcher.Dispatch(input);
            }
        }
    }
    //// End class
}
//// End namespace
