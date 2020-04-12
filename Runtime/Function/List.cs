using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function
{
    public class List : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length == 0)
                return Lisp.nil;
            Cons head = new Cons();
            Cons cur = head;
            bool t = false;
            foreach(var i in args)
            {
                if(t)
                {
                    cur.cdr = new Cons();
                    cur = cur.cdr as Cons;
                }
                t = true;
                cur.car = i;
            }
            return head;
        }
    }
    public class Car : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 1)
                throw new RuntimeException("CAR: exactly 1 argument required.");
            if (args[0] is null)
                return Lisp.nil;
            if (args[0] is Cons c)
            {
                return c.car;
            }
            else throw new RuntimeException("CAR: Invalid argument type");
        }
    }
    public class Cdr : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 1)
                throw new RuntimeException("CDR: Exactly 1 argument required");
            if(args[0] is null)
                return Lisp.nil;
            if (args[0] is Cons c)
                return c.cdr;
            else throw new RuntimeException("CDR: Invalid argument type");
        }
    }
}
