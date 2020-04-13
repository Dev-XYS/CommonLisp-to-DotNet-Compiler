using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    class LocalVariable : IL.Variable, IVariable
    {
        public LocalVariable(Environment e) : base(e) { }
        public LocalVariable(string name, Environment e) : base(name, e) { }

        public void Load(Function f)
        {
            f.Add(new IL.MoveInstruction(Global.rax, this));
        }

        public void Store(Function f)
        {
            f.Add(new IL.MoveInstruction(this, Global.rax));
        }
    }
    class SpecialVariable : IVariable
    {
        private static Dictionary<Symbol, SpecialVariable> gsl = new Dictionary<Symbol, SpecialVariable>();
        private Symbol name;
        private SpecialVariable(Symbol n)
        {
            name = n;
        }
        public static bool Declare(Symbol name)
        {
            if (!gsl.ContainsKey(name))
            {
                gsl[name] = new SpecialVariable(name);
                return true;
            }
            return false;
        }
        public static SpecialVariable Find(Symbol name)
        {
            SpecialVariable ret;
            if (gsl.TryGetValue(name, out ret)) return ret;
            return null;
        }
        public void Store(Function f)
        {
            f.Call(Global.env.Find(Symbol.Find("#SPECIAL-GET")), new IL.ImmediateNumber(name));
        }

        public void Load(Function f)
        {
            f.Call(Global.env.Find(Symbol.Find("#SPECIAL-SET")), new IL.ImmediateNumber(name), Global.rax);
        }
        public void Push(Function f, IL.IEntity value)
        {
            f.Call(Global.env.Find(Symbol.Find("#SPECIAL-PUSH")), new IL.ImmediateNumber(name), value);
        }
        public void Pop(Function f)
        {
            f.Call(Global.env.Find(Symbol.Find("#SPECIAL-POP")), new IL.ImmediateNumber(name));
        }
    }
}
