using Runtime;
using System;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            // var il = Frontend.Core.CompileFromStdin();
            CIL.Program prog = new CIL.Program(Test.IL.Closure.GenerateCode());
            prog.Emit();
        }
    }
}
