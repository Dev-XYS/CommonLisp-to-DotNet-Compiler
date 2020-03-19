using System;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            IILEntity entity;
            ILImmediateNumber imm = new ILImmediateNumber();
            ILVariable var = new ILVariable();
            entity = imm;
            Console.WriteLine(entity is ILImmediateNumber);
            Console.WriteLine(entity is ILVariable);
        }
    }
}
