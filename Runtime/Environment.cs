using Runtime.Function;
using Runtime.Function.Arith;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Runtime
{
    public class Environment
    {
        public Dictionary<Symbol, IType> d;
        public Environment outer;
        public Environment() { d = new Dictionary<Symbol, IType>(); }
        public Environment(Environment o) : this()
        {
            outer = o;
        }
        public IType Find(Symbol s)
        {
            IType ret;
            Environment cur = this;
            do
            {
                if (cur.d.TryGetValue(s, out ret))
                    return ret;
                cur = cur.outer;
            } while (cur != null);
            return null;
        }
        public static Environment MakeGlobalEnvironment()
        {
            Environment ret = new Environment();
            void AddOne(string name, IType value)
            {
                ret.d.Add(Symbol.FindOrCreate(name), value);
            }
            AddOne("T", Lisp.t);
            AddOne("NIL", Lisp.nil);
            AddOne("+", new Plus());
            AddOne("-", new Minus());
            AddOne("*", new Multiply());
            AddOne("/", new Divide());
            AddOne("<", new Less());
            AddOne(">", new Greater());
            AddOne("<=", new LessEqual());
            AddOne(">=", new GreaterEqual());
            AddOne("=", new NumberEqual());
            AddOne("/=", new NumberNotEqual());
            AddOne("NOT", new LogicNot());
            AddOne("EQ", new Eq());
            AddOne("EQL", new Eql());
            AddOne("LIST", new List());
            AddOne("CAR", new Car());
            AddOne("CDR", new Cdr());
            AddOne("LDIFF", new Ldiff());
            AddOne("NCONC", new Nconc());
            AddOne("READ", new Read());
            AddOne("WRITE", new Write());
            AddOne("WRITELN", new WriteLn());
            AddOne("#SPECIAL-GET", new DummyFunction());
            AddOne("#SPECIAL-SET", new DummyFunction());
            AddOne("#SPECIAL-PUSH", new DummyFunction());
            AddOne("#SPECIAL-POP", new DummyFunction());
            AddOne("#SPECIAL-RESET", new DummyFunction());
            return ret;
        }
    }
}
