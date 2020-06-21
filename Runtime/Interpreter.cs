using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Runtime
{
    public static class Interpreter
    {
        public static IType Eval(IType input, Environment e)
        {
            if(input is Symbol ss)
            {
                return e.Find(ss);
            }
            if(!(input is Cons c))
            {
                return input;
            }
            if (c.car is Symbol s)
            {
                if (s.Name == "DEFUN")
                    return Defun(c.cdr, e);
                if (s.Name == "LET")
                    return Let(c.cdr, e);
                if (s.Name == "LAMBDA")
                    return Lambda(c.cdr, e);
                if (s.Name == "QUOTE")
                    return Quote(c.cdr, e);
                if (s.Name == "IF")
                    return If(c.cdr, e);
                if (s.Name == "AND")
                    return And(c.cdr, e);
                if (s.Name == "OR")
                    return Or(c.cdr, e);
                if (s.Name == "SETQ")
                    return Setq(c.cdr, e);
                if (s.Name == "PROGN")
                    return Progn(c.cdr, e);
                return Funcall(Eval(s, e), c.cdr, e);
            }
            if (c.car is Cons cc)
            {
                return Funcall(Eval(cc, e), c.cdr, e);
            }
            else return null;
        }
        static IType Defun(IType input, Environment e)
        {
            var (tl, tbody) = Util.RequireAtLeast(input, 1, "DEFUN");
            if (!(tl[0] is Symbol s))
                throw new RuntimeException("{0} is not a valid name", tl[0]);
            IType f = Lambda(tbody, e);
            e.d[s] = f;
            return null;
        }
        static IType Progn(IType input, Environment e)
        {
            IType ret = null;
            while(input is Cons c)
            {
                ret = Eval(c.car, e);
                input = c.cdr;
            }
            return ret;
        }
        static IType Let(IType input, Environment e)
        {
            var (tl, tbody) = Util.RequireAtLeast(input, 1, "LET");
            IType[] bindings = Util.ListToArray(tl[0]);
            Environment ne = new Environment(e);
            foreach(IType i in bindings)
            {
                if(i is Symbol s)
                {
                    ne.d[s] = null;
                }else
                {
                    var curb = Util.ListToArray(i);
                    if (!(curb[0] is Symbol name))
                        throw new RuntimeException("{0} is not a valid name", curb[0]);
                    ne.d[name] = Eval(curb[1], e);
                }
            }
            return Progn(tbody, ne);
        }
        static IType Lambda(IType input, Environment e)
        {
            Func<IType[], IType> ret = (IType[] args) =>
            {
                Environment cur = new Environment(e);
                var (tl, tbody) = Util.RequireAtLeast(input, 1, "LAMBDA");
                IType[] names = Util.ListToArray(tl[0]);
                if (names.Length != args.Length) throw new RuntimeException("arguments mismatch!");
                for (int i = 0; i < names.Length; ++i)
                    if (names[i] is Symbol name && !name.Name.StartsWith("&"))
                        cur.d[name] = args[i];
                    else throw new RuntimeException("{0} is not a valid name", names[i]);
                return Progn(tbody, cur);
            };
            return new DynamicFunction(ret);
        }
        public static Func<IType, IType> Defmacro(IType input)
        {
            var (tl, tr) = Util.RequireAtLeast(input, 1, "DEFMACRO");
            IType[] nms = Util.ListToArray(tl[0]);
            Symbol name = null;
            for(int i = 0; i < nms.Length; ++i)
                if(nms[i] is Symbol n && !n.Name.StartsWith("&"))
                {
                    if (name != null) throw new RuntimeException("DEFMACRO: Exactly 1 parameter required.");
                    name = n;
                }
            Func<IType, IType> ret = (IType arg) =>
            {
                Environment cur = new Environment(Lisp.global);
                cur.d[name] = arg;
                return Progn(tr, cur);
            };
            return ret;
        }
        static IType Quote(IType input, Environment e)
        {
            return (input as Cons).car;
        }
        static IType If(IType input, Environment e)
        {
            var tl = Util.RequireExactly(input, 3, "IF");
            if (Eval(tl[0], e) is null)
                return Eval(tl[2], e);
            else
                return Eval(tl[1], e);
        }
        static IType And(IType input, Environment e)
        {
            while(input is Cons c)
            {
                if (Eval(c.car, e) is null)
                    return null;
                input = c.cdr;
            }
            return Lisp.t;
        }
        static IType Or(IType input, Environment e)
        {
            while(input is Cons c)
            {
                if (Eval(c.car, e) != null)
                    return Lisp.t;
                input = c.cdr;
            }
            return null;
        }
        static IType Setq(IType input, Environment e)
        {
            if(input is Cons c)
            {
                if (c.car is Symbol s)
                {
                    if (c.cdr is Cons d)
                    {
                        e.Set(s, Eval(d.car, e));
                        Setq(d.cdr, e);
                    }
                }
                else throw new RuntimeException("{0} is not SETQable", c.car);
            }
            return null;
        }
        static IType Funcall(IType func, IType param, Environment e)
        {
            var args = Util.ListToArray(param);
            for (int i = 0; i < args.Length; ++i)
                args[i] = Eval(args[i], e);
            return func.Invoke(args);
        }
    }
}
