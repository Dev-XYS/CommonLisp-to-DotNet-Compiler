using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    class Environment : IL.Environment
    {
        public static List<Environment> gel = new List<Environment>();
        public Dictionary<Symbol, int> pos;
        public Environment outer;
        public Environment() : base()
        {
            pos = new Dictionary<Symbol, int>();
            outer = this;
            gel.Add(this);
        }
        public Environment(Environment o) : base()
        {
            pos = new Dictionary<Symbol, int>();
            outer = o;
            gel.Add(this);
        }
        public LocalVariable AddVariable(Symbol s)
        {
            if (pos.ContainsKey(s))
                throw new SyntaxError(string.Format("Env: Redefined name {0}", s));
            pos.Add(s, VariableList.Count);
            var ret = new LocalVariable(s.Name, this);
            VariableList.Add(ret);
            return ret;
        }
        public LocalVariable AddUnnamedVariable()
        {
            var ret = new LocalVariable(this);
            VariableList.Add(ret);
            return ret;
        }
        public LocalVariable Find(Symbol s)
        {
            int p;
            if (pos.TryGetValue(s, out p))
            {
                return VariableList[p] as LocalVariable;
            }
            else if (outer != this)
            {
                return outer.Find(s);
            }
            else throw new SyntaxError(string.Format("{0} Not Found", s));
        }
    }
}
