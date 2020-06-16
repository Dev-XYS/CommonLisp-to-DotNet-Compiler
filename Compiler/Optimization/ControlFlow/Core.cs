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

                Console.WriteLine("\n--- graph of {0} ---\n", func.ILFunction.Name);
                graph.Print();

                graph.Optimize();

                graph.PrintDAGs();
            }
            return program;
        }
    }
}
