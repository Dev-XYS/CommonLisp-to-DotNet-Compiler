using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public static class Printer
    {
        public static void Write(IType obj)
        {
            if (obj is Cons c)
            {
                Console.Write("(");
                while (true)
                {
                    Write(c.car);
                    if (c.cdr is Cons d)
                    {
                        c = d;
                        Console.Write(" ");
                    }
                    else if (c.cdr is null)
                    {
                        Console.Write(")");
                        break;
                    }
                    else
                    {
                        Console.Write(" . ");
                        Write(c.cdr);
                        Console.Write(")");
                        break;
                    }
                }
            }
            else if (obj is null)
                Console.Write("NIL");
            else Console.Write(obj);
        }
        public static void WriteLine(IType obj)
        {
            Write(obj);
            Console.WriteLine();
        }
    }
}
