using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    class Environment : Class
    {
        public override string Name { get; }

        public override string CtorArgumentList
        {
            get => "";
        }

        public Dictionary<IL.Variable, ITypeMember> VarMap { get; }

        public Environment(IL.Environment env)
        {
            Name = "env" + env.GetHashCode().ToString();
            VarMap = new Dictionary<IL.Variable, ITypeMember>();

            foreach (IL.Variable var in env.VariableList)
            {
                VarMap[var] = new ITypeMember(this, var);
            }
        }

        public void Emit()
        {
            Emitter.Emit(".class private auto ansi beforefieldinit {0} extends [System.Runtime]System.Object", Name);
            Emitter.BeginBlock();
            foreach (ITypeMember m in VarMap.Values)
            {
                Emitter.Emit(".field public class [Runtime]Runtime.IType {0}", m.Name);
            }
            Emitter.Emit(".method public hidebysig specialname rtspecialname instance void .ctor() cil managed\n\t{{\n\t\t.maxstack 8\n\t\tldarg.0\n\t\tcall instance void [System.Runtime]System.Object::.ctor()\n\t\tret\n\t}}");
            Emitter.EndBlock();
        }
    }
}
