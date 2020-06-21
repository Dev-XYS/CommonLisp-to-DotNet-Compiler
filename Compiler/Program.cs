using Runtime;
using System;

namespace Compiler
{
    static class Program
    {
        private enum Mode
        {
            Program,
            Library,
            Interpreter,
            REPL
        }

        private static Mode CompileMode { get; set; }
        private static string Input { get; set; }

        public static void Main(string[] args)
        {
            if (!ParseArgs(args))
            {
                Console.WriteLine("unknown option");
                Console.WriteLine("usage: Compiler.exe [file|-l [file]|-i|-ii]");
                return;
            }

            if (CompileMode == Mode.Program)
            {
                CompileProgram();
            }
            else if (CompileMode == Mode.Library)
            {
                CompileLibrary();
            }
            else if (CompileMode == Mode.Interpreter)
            {
                Frontend.Core.Interpret();
            }
            else
            {
                Frontend.Core.Interpret(true);
            }
        }

        private static bool ParseArgs(string[] args)
        {
            foreach (string arg in args)
            {
                Mode mode = Mode.Program;

                if (arg == "-l")
                {
                    mode = Mode.Library;
                }
                else if (arg == "-i")
                {
                    mode = Mode.Interpreter;
                }
                else if (arg == "-ii")
                {
                    mode = Mode.REPL;
                }
                else
                {
                    if (Input == null)
                    {
                        Input = arg;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (CompileMode == Mode.Program)
                {
                    CompileMode = mode;
                }
            }

            if (CompileMode != Mode.Program && CompileMode != Mode.Library && Input != null)
            {
                return false;
            }

            return true;
        }

        private static void CompileProgram()
        {
            IL.Program prog;
            if (Input == null)
            {
                prog = Frontend.Core.CompileFromStdin();
            }
            else
            {
                prog = Frontend.Core.CompileFromFile(Input);
            }
            Optimization.Program optProg = Optimization.Core.Optimize(prog);
            var cilProg = new CIL.Program(optProg);
            var sw = new System.IO.StreamWriter("Program.il");
            cilProg.Emit(sw, CIL.EmissionType.Program);
            sw.Close();
            Assembler.Assembler.Invoke("Program.il");
        }

        private static void CompileLibrary()
        {
            IL.Program prog;
            if (Input == null)
            {
                prog = Frontend.Core.CompileFromStdin();
            }
            else
            {
                prog = Frontend.Core.CompileFromFile(Input);
            }
            prog.Main.Name = "LibMain";
            var optProg = Optimization.DummyOptimization.Optimize(prog);
            var cilProg = new CIL.Program(optProg);
            var sw = new System.IO.StreamWriter("Library.il");
            cilProg.Emit(sw, CIL.EmissionType.Library);
            sw.Close();
            Assembler.Assembler.Invoke("Library.il");
        }

        private static void PreCompile()
        {
            return;
        }
    }
}
