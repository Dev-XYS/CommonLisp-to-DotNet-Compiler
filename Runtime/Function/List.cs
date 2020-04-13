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
    public class Nconc: IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length == 0)
                return Lisp.nil;
            if (!Array.TrueForAll(args, (IType x) => x is Cons || x is null))
                throw new RuntimeException("NCONC: Invalid argument type");
            Cons[] c = Array.ConvertAll(args, (IType x) => x as Cons);
            Cons ret = c[0];
            for(int i = 0; i < c.Length-1; ++i)
            {
                while (c[i].cdr is Cons d)
                    c[i] = d;
                c[i].cdr = c[i + 1];
            }
            return ret;
        }
    }
    public class Ldiff: IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 2)
                throw new RuntimeException("LDIFF: Exactly 2 arguments required");
            if (!Array.TrueForAll(args, (IType x) => x is null || x is Cons)) throw new RuntimeException("LDIFF: Invalid Argument type");
            if (args[0] is null)
                return null;
            Cons c = args[0] as Cons;
            if (c.Equals(args[1])) return null;
            Cons ret = new Cons(c.car, Lisp.nil);
            Cons cur = ret;
            while(c.cdr is Cons d && !d.Equals(args[1]))
            {
                cur.cdr = new Cons(d.car, Lisp.nil);
                cur = cur.cdr as Cons;
                c = d;
            }
            return ret;
        }
    }
}
