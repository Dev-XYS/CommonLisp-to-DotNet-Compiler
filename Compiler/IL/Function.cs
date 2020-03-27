using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class Function : IEntity
    {
        public string Name;
        public List<IInstruction> InstructionList;

        public Function(string name)
        {
            Name = name;
            InstructionList = new List<IInstruction>();
        }
    }

    class ParametersFunction : Function
    {
        public ParametersFunction(string name) : base(name)
        {

        }
    }

    class ListFunction : Function
    {
        public ListFunction(string name) : base(name)
        {
        }
    }
}
