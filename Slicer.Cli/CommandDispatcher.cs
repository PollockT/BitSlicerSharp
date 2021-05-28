using Slicer.Cli.CommandHandlers;
using Slicer.Cli.CommandHandlers.Crash;
using Slicer.Cli.CommandHandlers.Process;
using Slicer.Cli.CommandHandlers.Project;
using Slicer.Cli.CommandHandlers.Projects;
using Slicer.Cli.CommandHandlers.Results;
using Slicer.Cli.CommandHandlers.Scan;

namespace Slicer.Cli
{
    using Cli.CommandHandlers;
    using Cli.CommandHandlers.Crash;
    using Cli.CommandHandlers.Process;
    using Cli.CommandHandlers.Project;
    using Cli.CommandHandlers.Projects;
    using Cli.CommandHandlers.Results;
    using Cli.CommandHandlers.Scan;
    using Engine;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandDispatcher
    {
        private Session session;

        public CommandDispatcher()
        {
            this.CommandHandlers = new List<ICommandHandler>()
            {
                new CrashCommandHandler(),
                new ProcessCommandHandler(),
                new ProjectsCommandHandler(),
                new ProjectCommandHandler(),
                new ScanCommandHandler(),
                new ResultsCommandHandler(),
            };
        }

        private Session Session
        {
            get
            {
                return this.session;
            }

            set
            {
                this.session = value;
            }
        }

        List<ICommandHandler> CommandHandlers { get; set; }

        public void Dispatch(String command)
        {
            Command constructedCommand = new Command(command);

            this.TryHandleHelp(constructedCommand);

            foreach (ICommandHandler handler in this.CommandHandlers)
            {
                if (constructedCommand.Handled)
                {
                    break;
                }

                if (constructedCommand.Name.Equals(handler.GetCommandName(), StringComparison.OrdinalIgnoreCase)
                    || handler.GetCommandAndAliases().Any(alias => constructedCommand.Name.Equals(alias, StringComparison.OrdinalIgnoreCase)))
                {
                    handler.TryHandle(ref this.session, constructedCommand);
                }
            }

            if (!constructedCommand.Handled)
            {
                if (!String.IsNullOrWhiteSpace(constructedCommand.Name))
                {
                    Console.WriteLine("Unrecognized command '" + constructedCommand.Name + "'");
                }
            }
        }

        private void TryHandleHelp(Command command)
        {
            if (!command.Name.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            foreach (ICommandHandler handler in this.CommandHandlers)
            {
                Console.Write("Commands for " + handler.GetCommandName() + ": ");
                Console.Write(handler.GetCommandName().ToLower() + " | ");

                foreach (String alias in handler.GetCommandAndAliases())
                {
                    Console.Write(alias.ToLower() + " | ");
                }

                Console.WriteLine();
            }

            command.Handled = true;
        }
    }
    //// End class
}
//// End namespace
