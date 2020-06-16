using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization.ControlFlow
{
    class BasicBlock
    {
        private List<IL.Instruction> InstructionList { get; }

        private List<BasicBlock> Successor { get; }

        public IL.Instruction Leader { get => InstructionList[0]; }

        public IL.Instruction LeavingInstruction { get => InstructionList[InstructionList.Count - 1]; }

        public BasicBlock()
        {
            InstructionList = new List<IL.Instruction>();
            Successor = new List<BasicBlock>();
        }

        public BasicBlock(List<IL.Instruction> list)
        {
            InstructionList = list;
        }

        public void AddInstruction(IL.Instruction instr)
        {
            InstructionList.Add(instr);
        }

        public void AddSuccessor(BasicBlock block)
        {
            Successor.Add(block);
        }

        public void Print()
        {
            Console.WriteLine("--- {0} ---", GetHashCode());
            foreach (IL.Instruction instr in InstructionList)
            {
                Console.WriteLine(instr);
            }
            Console.WriteLine(">>>");
            foreach (BasicBlock block in Successor)
            {
                Console.WriteLine(block.GetHashCode());
            }
            Console.WriteLine("<<<");
        }
    }
}
