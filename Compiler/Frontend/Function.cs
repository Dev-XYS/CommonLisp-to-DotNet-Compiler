using System;
using System.Collections.Generic;
using System.Text;
using Runtime;

namespace Compiler.Frontend
{
    class Function : IL.ParametersFunction
    {
        public static List<Function> gfl = new List<Function>();
        private HashSet<Symbol> Params { get; }
        private Environment Env { get; }
        public Function(Environment e) : base()
        {
            Env = e;
            Params = new HashSet<Symbol>();
            EnvList.Add(e);
            while(e != e.outer)
            {
                e = e.outer;
                EnvList.Add(e);
            }
            gfl.Add(this);
        }
        public void AddParam(Symbol s)
        {
            Parameters.Add(Env.AddVariable(s));
            Params.Add(s);
        }
        public bool ContainsParam(Symbol s)
        {
            return Params.Contains(s);
        }
        public IVariable FindVar(Symbol s)
        {
            if (ContainsParam(s)) return Env.Find(s);
            IVariable ret = SpecialVariable.Find(s);
            if (ret is null)
                return Env.Find(s);
            return ret;
        }
        public void Load(IVariable v)
        {
            v.Load(this);
        }
        public void Store(object o)
        {
            if (o is null)
                Add(new IL.MoveInstruction(new IL.ImmediateNumber(null), Global.rax));
            else if (o is IVariable v)
                v.Store(this);
            else if (o is IType t)
                Add(new IL.MoveInstruction(new IL.ImmediateNumber(t), Global.rax));
            else if (o is Function f)
                Add(new IL.FunctionInstruction(f, Global.rax));
            else throw new Exception("Store: Invalid call");
        }
        public void Return()
        {
            Add(new IL.ReturnInstruction(Global.rax));
        }
        public void Call(LocalVariable f, params IL.IEntity[] args)
        {
            var ins = new IL.CallInstruction(f, Global.rax);
            ins.Parameters.AddRange(args);
            Add(ins);
        }
    }
}
