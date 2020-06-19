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

                Console.WriteLine("\n--- graph of {0} (unoptimized) ---\n", func.ILFunction.Name);
                graph.Print();

                graph.Optimize();

                graph.PrintDAGs();

                func.InstructionList = graph.ReassembleInstructions();

                Console.WriteLine("\n--- graph of {0} (optimized) ---\n", func.ILFunction.Name);
                graph.Print();

                LivenessAnalysis.Core.Analyze(graph);

                Console.WriteLine("--- optimized instruction list of {0} ---\n", func.ILFunction.Name);
                foreach (IL.Instruction instr in func.InstructionList)
                {
                    Console.WriteLine(instr);
                }
            }
            return program;
        }
    }
}
