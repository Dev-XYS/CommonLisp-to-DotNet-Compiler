using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    abstract class Function
    {
        public string Name { get; set; }

        public List<Environment> EnvList { get; }

        public List<Variable> Parameters { get; }

        public List<IInstruction> InstructionList { get; }

        public Function()
        {
            EnvList = new List<Environment>();
            Parameters = new List<Variable>();
            InstructionList = new List<IInstruction>();
        }
    }

    class ParametersFunction : Function
    {
        public ParametersFunction() : base()
        {

        }
    }

    class ListFunction : Function
    {
        public ListFunction() : base()
        {
        }
    }
}
