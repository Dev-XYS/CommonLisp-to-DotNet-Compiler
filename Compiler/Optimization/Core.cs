using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    static class Core
    {
        static private IL.Program Program;

        static private Program OptimizedProgram;

        static public Program Optimize(IL.Program program)
        {
            Program = program;
            OptimizedProgram = new Program(Program);
            OptimizedProgram = LocalOptimization.Optimize(OptimizedProgram);
            return OptimizedProgram;
        }
    }
}
