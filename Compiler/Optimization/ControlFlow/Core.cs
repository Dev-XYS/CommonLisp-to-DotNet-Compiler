using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization.ControlFlow
{
    static class Core
    {
        static public Program Optimize(Program program)
        {
            foreach (Function func in program.FunctionList)
            {
                Graph graph = new Graph(func);

                graph.OptimizeLocally();

                func.InstructionList = graph.ReassembleInstructions();

                // Liveness analysis.
                graph = new Graph(func);
                LivenessAnalysis.Core.Analyze(graph);
                graph.OptimizeLocally();
                func.InstructionList = graph.ReassembleInstructions();
            }
            return program;
        }
    }
}
