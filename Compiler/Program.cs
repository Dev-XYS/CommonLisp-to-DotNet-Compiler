using Runtime;
using System;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = Frontend.Core.CompileFromStdin();
            //do sth with program
        }
    }
}
