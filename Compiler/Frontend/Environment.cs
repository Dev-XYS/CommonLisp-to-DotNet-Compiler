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
        public LocalVariable rax;
        public Environment(Environment o = null) : base()
        {
            pos = new Dictionary<Symbol, int>();
            outer = o;
            gel.Add(this);
            rax = AddUnnamedVariable();
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
            else if (!(outer is null))
            {
                return outer.Find(s);
            }
            else throw new SyntaxError(string.Format("{0} Not Found", s));
        }
        public IL.IEntity FindOrExtern(Symbol s)
        {
            try
            {
                return Find(s);
            }catch(SyntaxError e)
            {
                return new IL.UnresolvedObject(s.Name);
            }
        }
    }
}
