using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function
{
    public class Write : IType
    {
        public IType Invoke(IType[] args)
        {
            bool first = true;
            foreach(var i in args)
            {
                if (first) first = false;
                else Console.Write(" ");
                Printer.Write(i, Lisp.stdout);
            }
            return Lisp.nil;
        }
    }
}
