using Runtime;
using System;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            IL.Program program;
            if (args.Length > 0)
            {
                program = Frontend.Core.CompileFromFile(args[0]);
            }
            else
            {
                program = Frontend.Core.CompileFromStdin();
            }
            Optimization.Program OptimizedProgram = Optimization.Core.OptimizeILProgram(program);
            var prog = new CIL.Program(OptimizedProgram);
            var sw = new System.IO.StreamWriter("Program.il");
            prog.Emit(sw, CIL.EmissionType.Program);
            sw.Close();
            Assembler.Assembler.Invoke("Program.il");
        }
    }
}
