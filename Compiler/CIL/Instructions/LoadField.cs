using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class LoadField : Instruction
    {
        public Member Field { get; set; }

        public override void Emit()
        {
            if (Field is EnvironmentMember)
            {
                EnvironmentMember f = Field as EnvironmentMember;
                Emitter.Emit("ldfld class {0} {1}::{2}", f.Type, f.Function.Name, f.Name);
            }
            else
            {
                ITypeMember f = Field as ITypeMember;
                Emitter.Emit("ldfld class {0} {1}::'{2}'", f.Type, f.Environment.Name, f.Name);
            }
        }
    }
}
