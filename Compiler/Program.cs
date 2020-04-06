using Runtime;
using System;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var il = Frontend.Core.CompileFromStdin();
            //do sth with il
            Console.WriteLine(il);
        }
    }
}
