using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    class Environment
    {
        public IL.Environment ilenv;
        public Dictionary<Symbol, int> pos;
        public Environment outer;
        public Environment()
        {
            ilenv = new IL.Environment();
            pos = new Dictionary<Symbol, int>();
            outer = this;
        }
        public Environment(Environment o, int _)
        {
            ilenv = new IL.Environment();
            pos = new Dictionary<Symbol, int>();
            outer = o;
        }
        public void AddVariable(Symbol s)
        {
            pos.Add(s, ilenv.VariableList.Count);
            ilenv.VariableList.Add(new IL.Variable(ilenv));
        }
        public IL.Variable Find(Symbol s)
        {
            int p;
            if (pos.TryGetValue(s, out p))
            {
                return ilenv.VariableList[p];
            }
            else if (outer != this)
            {
                return outer.Find(s);
            }
            else throw new SyntaxError(string.Format("{0} Not Found", s));
        }
    }
}
