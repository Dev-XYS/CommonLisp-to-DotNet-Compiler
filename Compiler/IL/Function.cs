using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    abstract class Function
    {
        private static int gfid;
        private int fid;

        public string Name { get; set; }

        public List<Environment> EnvList { get; }

        public List<Variable> Parameters { get; }

        public List<Instruction> InstructionList { get; }

        public Environment LocalEnv { get => EnvList[0]; }

        public Function()
        {
            fid = gfid++;
            Name = "func" + fid;

            EnvList = new List<Environment>();
            Parameters = new List<Variable>();
            InstructionList = new List<Instruction>();
        }
        public void Add(Instruction i)
        {
            InstructionList.Add(i);
        }

        public override int GetHashCode()
        {
            return fid;
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
