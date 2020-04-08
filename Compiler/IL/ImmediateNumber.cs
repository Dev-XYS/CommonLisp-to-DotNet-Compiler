using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class ImmediateNumber : IEntity
    {
        public Runtime.IType Imm { get; set; }
        public ImmediateNumber(Runtime.IType i)
        {
            Imm = i;
        }

        public override string ToString()
        {
            return Imm == null ? "null" : Imm.ToString();
        }
    }
}
