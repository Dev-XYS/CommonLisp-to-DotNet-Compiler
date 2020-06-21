using Runtime;
using System;

namespace Compiler
{
    class Program
    {
        static void CompileLibrary()
        {
            var prog = Frontend.Core.CompileFromStdin();
            prog.Main.Name = "LibMain";
            var optProg = Optimization.DummyOptimization.Optimize(prog);
            var cilprog = new CIL.Program(optProg);
            var sw = new System.IO.StreamWriter("Library.il");
            cilprog.Emit(sw, CIL.EmissionType.Library);
            sw.Close();
            Assembler.Assembler.Invoke("Library.il");
        }
        static void PreCompile()
        {
            return;
        }
        static void Main(string[] args)
        {
            IL.Program program;
            if (args.Length == 1)
            {
                if (args[0] == "-l")
                {
                    CompileLibrary();
                    return;
                }
                if (args[0] == "-p")
                {
                    PreCompile();
                    return;
                }
                if(args[0] == "-i")
                {
                    Frontend.Core.Interpret();
                    return;
                }
                program = Frontend.Core.CompileFromFile(args[0]);
            }
            else
            {
                program = Frontend.Core.CompileFromStdin();
            }
            Optimization.Program OptimizedProgram = Optimization.Core.Optimize(program);
            var prog = new CIL.Program(OptimizedProgram);
            var sw = new System.IO.StreamWriter("Program.il");
            prog.Emit(sw, CIL.EmissionType.Program);
            sw.Close();
            Assembler.Assembler.Invoke("Program.il");
        }
    }
}
