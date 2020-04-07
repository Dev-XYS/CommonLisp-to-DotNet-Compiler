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
            prog.Emit();
        }
    }
}
