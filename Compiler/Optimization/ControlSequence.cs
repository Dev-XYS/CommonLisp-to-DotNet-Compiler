using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    class ControlSequence
    {
        public List<Instruction> Instructions { get; }

        private IL.Function ILFunction { get; }

        public ControlSequence(List<IL.Instruction> instrList, IL.Function func)
        {
            Instructions = new List<Instruction>();
            ILFunction = func;

            foreach (IL.Instruction instr in instrList)
            {
                Instructions.Add(new Instruction(instr, func));
            }
        }
    }
}
