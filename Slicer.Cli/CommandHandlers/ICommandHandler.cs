namespace Slicer.Cli.CommandHandlers
{
    using Engine;
    using System;
    using System.Collections.Generic;

    public interface ICommandHandler
    {
        String GetCommandName();

        void TryHandle(ref Session session, Command command);

        IEnumerable<String> GetCommandAndAliases();
    }
    //// End class
}
//// End namespace
