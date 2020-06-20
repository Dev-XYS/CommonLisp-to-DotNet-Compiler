using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    partial class Function
    {
        public IL.Function ILFunction { get; }

        public string Name { get; set; }

        public List<IL.Environment> EnvList { get; }

        public List<IL.Variable> Parameters { get; }

        public List<IL.Instruction> InstructionList { get; set; }

        public IL.Environment LocalEnv { get => EnvList[0]; }

        public Function(IL.Function func)
        {
            ILFunction = func;
            Name = func.Name;
            EnvList = func.EnvList;
            Parameters = func.Parameters;
            InstructionList = func.InstructionList;
            Loc = new Dictionary<IL.IEntity, LocalVariable>();
        }

        public void Print()
        {
            Console.WriteLine("--- function: {0} ---", Name);
            foreach (IL.Instruction instr in InstructionList)
            {
                Console.WriteLine(instr);
            }
            Console.WriteLine();
        }
    }
}
