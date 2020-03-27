using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class ImmediateNumber : IEntity
    {
        public Runtime.IType Imm { get; set; }
    }
}
