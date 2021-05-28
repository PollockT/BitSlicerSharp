using Slicer.Engine.Architecture.Disassemblers;

namespace Slicer.Engine.Architecture
{
    using Architecture.Disassemblers;

    public static class Disassembler
    {
        public static IDisassembler Default
        {
            get
            {
                return DisassemblerFactory.GetDisassembler(ArchitectureType.x86_64);
            }
        }

        public static IDisassembler X86_64
        {
            get
            {
                return DisassemblerFactory.GetDisassembler(ArchitectureType.x86_64);
            }
        }
    }
    //// End class
}
//// End namespace