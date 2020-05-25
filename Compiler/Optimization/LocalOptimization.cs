using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    static class LocalOptimization
    {
        static private IL.Program Program;

        static public IL.Program Optimize(IL.Program program)
        {
            Program = program;
            OptimizeAllFunctions();
            return Program;
        }

        static private void OptimizeAllFunctions()
        {

        }
    }
}
