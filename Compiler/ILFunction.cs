using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    class ILFunction
    {
        public string Name;
        public List<IILInstruction> InstructionList;

        public ILFunction(string name)
        {
            Name = name;
            InstructionList = new List<IILInstruction>();
        }
    }

    class ILParametersFunction : ILFunction
    {
        public ILParametersFunction(string name) : base(name)
        {

        }
    }

    class ILListFunction : ILFunction
    {
        public ILListFunction(string name) : base(name)
        {
        }
    }
}
