using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    class ILImmediateNumber : IILEntity
    {
        public Runtime.IType Imm { get; set; }
    }
}
