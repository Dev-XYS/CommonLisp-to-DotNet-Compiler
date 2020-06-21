using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    static class Macro
    {
        private static Dictionary<Symbol, Func<IType, IType>> gml = new Dictionary<Symbol, Func<IType, IType>>();
        public static void Init(string ppath)
        {
            gml.Add(Symbol.FindOrCreate("DEFPARAMETER"), (IType form)=>
            {
                var (t1, tbody) = Util.RequireAtLeast(form, 1, "DEFPARAMETER");
                if (!(t1[0] is Symbol name))
                    throw new SyntaxError("DEFPARAMETER: Invalid name");
                Cons ret = new Cons(Symbol.FindOrCreate("PROGN"), new Cons(
                    new Cons(Symbol.FindOrCreate("SPECIAL"), new Cons(name, Lisp.nil)), Lisp.nil));
                if (tbody is Cons c)
                {
                    (ret.cdr as Cons).cdr = new Cons(new Cons(Symbol.FindOrCreate("SETQ"), new Cons(name, new Cons(c.car, Lisp.nil))), Lisp.nil);
                }
                return ret;
            });
            gml.Add(Symbol.FindOrCreate("AND"), (IType form) =>
            {
                if (!(form is Cons c))
                    return Symbol.FindOrCreate("T");
                return new Cons(Symbol.FindOrCreate("IF"), new Cons(c.car, new Cons(new Cons(Symbol.FindOrCreate("AND"), c.cdr), new Cons(Symbol.FindOrCreate("NIL"), Lisp.nil))));
            });
            gml.Add(Symbol.FindOrCreate("OR"), (IType form) =>
            {
                if (!(form is Cons c))
                    return Symbol.FindOrCreate("NIL");
                return new Cons(Symbol.FindOrCreate("IF"), new Cons(c.car, new Cons(Symbol.FindOrCreate("T"), new Cons(new Cons(Symbol.FindOrCreate("OR"), c.cdr), Lisp.nil))));
            });
        }
        public static bool IsMacro(Symbol s)
        {
            return gml.ContainsKey(s);
        }
        public static IType Expand(Cons form)
        {
            return gml[form.car as Symbol](form.cdr);
        }
        public static void Register(Symbol s, Func<IType, IType> f)
        {
            if (gml.ContainsKey(s))
                throw new SyntaxError(string.Format("DEFMACRO: {0} redefined", s));
            gml[s] = f;
        }
    }
}
