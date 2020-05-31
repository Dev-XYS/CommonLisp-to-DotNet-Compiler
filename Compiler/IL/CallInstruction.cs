using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class CallInstruction : Instruction
    {
        public IEntity Function { get; set; }
        public Variable Destination { get; set; }
        public List<IEntity> Parameters { get; }

        public CallInstruction(IEntity function, Variable destination)
        {
            Function = function;
            Destination = destination;
            Parameters = new List<IEntity>();
        }

        public override string ToString()
        {
            return string.Format("[CALL] {0} ({1}) -> {2}", Function.ToString(), GetParameterList(), Destination.ToString());
        }

        private string GetParameterList()
        {
            return string.Join(", ", Parameters.ConvertAll((IEntity e) => e.ToString()));
        }

        public override Variable DefinedVariable
        {
            get
            {
                return Destination;
            }
        }

        public override List<Variable> UsedVariables
        {
            get
            {
                List<Variable> r = new List<Variable>();
                if (Function is Variable f)
                {
                    r.Add(f);
                }
                foreach (IEntity e in Parameters)
                {
                    if (e is Variable v)
                    {
                        r.Add(v);
                    }
                }
                return r;
            }
        }
    }
}
