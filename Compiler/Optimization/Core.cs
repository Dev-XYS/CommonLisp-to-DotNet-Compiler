using Compiler.Optimization.ControlFlow;
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
            foreach (Function func in OptimizedProgram.FunctionList)
            {
                Graph graph = new Graph(func);
                graph.Print();
            }
            OptimizedProgram = LocalOptimization.Optimize(OptimizedProgram);
            return OptimizedProgram;
        }
    }
}
