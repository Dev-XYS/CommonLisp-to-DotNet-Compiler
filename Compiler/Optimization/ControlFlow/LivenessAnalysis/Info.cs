using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization.ControlFlow.LivenessAnalysis
{
    class Info
    {
        public HashSet<IL.Variable> In { get; set; }

        public HashSet<IL.Variable> Out { get; set; }

        public HashSet<IL.Variable> Def { get; }

        public HashSet<IL.Variable> Use { get; }

        public Info()
        {
            In = new HashSet<IL.Variable>();
            Out = new HashSet<IL.Variable>();
            Def = new HashSet<IL.Variable>();
            Use = new HashSet<IL.Variable>();
        }

        public void AddDef(IL.Variable var)
        {
            if (var != null)
            {
                Def.Add(var);
            }
        }

        public void AddUse(List<IL.Variable> vars)
        {
            Use.UnionWith(vars);
        }
    }
}
