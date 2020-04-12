using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public static class Printer
    {
        public static void Write(IType obj, IOutputStream output)
        {
            if (obj is Cons c)
            {
                output.PutS("(");
                while (true)
                {
                    Write(c.car, output);
                    if (c.cdr is Cons d)
                    {
                        c = d;
                        output.PutChar(' ');
                    }
                    else if (c.cdr is null)
                    {
                        output.PutChar(')');
                        break;
                    }
                    else
                    {
                        output.PutS(" . ");
                        Write(c.cdr, output);
                        output.PutChar(')');
                        break;
                    }
                }
            }
            else if (obj is null)
                output.PutS("NIL");
            else output.PutS(obj.ToString());
        }
        public static void WriteLine(IType obj, IOutputStream output)
        {
            Write(obj, output);
            output.PutChar('\n');
        }
    }
}
