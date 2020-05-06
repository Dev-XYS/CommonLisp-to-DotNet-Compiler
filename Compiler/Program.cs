using Runtime;
using System;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = Frontend.Core.CompileFromStdin();
            var prog = new CIL.Program(program);
            var sw = new System.IO.StreamWriter("temp.il");
            prog.Emit(sw, CIL.EmissionType.Library);
            sw.Close();
            Assembler.Assembler.Invoke("temp.il");
        }
    }
}
