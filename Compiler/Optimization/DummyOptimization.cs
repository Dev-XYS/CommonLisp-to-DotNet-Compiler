using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    static class DummyOptimization
    {
        public static Program Optimize(IL.Program program)
        {
            return new Program(program);
        }
    }
}
