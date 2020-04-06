using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function
{
    public class WriteLn : IType
    {
        public IType Invoke(IType[] args)
        {
            bool first = true;
            foreach (var i in args)
            {
                if (first) first = false;
                else Console.Write(" ");
                Printer.Write(i);
            }
            Console.WriteLine();
            return Lisp.nil;
        }
    }
}
