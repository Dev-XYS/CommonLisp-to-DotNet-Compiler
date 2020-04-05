using Runtime;
using System;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Lisp.Init();
            IType t;
            while (true)
            {
                try
                {
                    t = Reader.Read(Lisp.stdin);
                    Console.WriteLine(t);
                }catch(Reader.EOFError)
                {
                    break;
                }
            }
        }
    }
}
