using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Compiler.Optimization
{
    class Variable
    {
        public IL.Variable ILVariable { get; }

        public Variable(IL.Variable var)
        {
            ILVariable = var;
        }

        static public HashSet<IL.Variable> NonLocalVariables { get; } = new HashSet<IL.Variable>();
    }
}
