using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    static class Core
    {
        static private IL.Program Program;

        static public IL.Program OptimizeILProgram(IL.Program program)
        {
            Program = program;
            Program = PeepHole.Optimize(Program);
            return Program;
        }
    }
}
