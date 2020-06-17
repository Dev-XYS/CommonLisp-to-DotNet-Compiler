using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Optimization.ControlFlow
{
    class Graph
    {
        private BasicBlock Entry { get; }

        private Dictionary<IL.Instruction, BasicBlock> LookupByLeader { get; }

        // Preserve the original order.
        private List<BasicBlock> BlockList { get; }

        public Graph(Function func)
        {
            LookupByLeader = new Dictionary<IL.Instruction, BasicBlock>();
            BlockList = new List<BasicBlock>();

            // Mark if an instruction is the leader of a basic block.
            bool[] IsLeader = new bool[func.InstructionList.Count];

            IsLeader[0] = true;

            for (int i = 0; i < IsLeader.Length; i++)
            {
                IL.Instruction instr = func.InstructionList[i];

                if (instr is IL.JumpInstruction)
                {
                    // The current instruction is a jump instruction.
                    // The next instruction is a leader.
                    if (i + 1 < func.InstructionList.Count)
                    {
                        IsLeader[i + 1] = true;
                    }
                }
                //else if (instr is IL.CallInstruction)
                //{
                //    // The current instruction is a call instruction.
                //    // Call instructions are leaders.
                //    IsLeader[i] = true;

                //    // The next instruction is also a leader.
                //    // Call takes up an entire block.
                //    if (i + 1 < func.InstructionList.Count)
                //    {
                //        IsLeader[i + 1] = true;
                //    }
                //}
                else if (instr is IL.Label)
                {
                    IsLeader[i] = true;
                }
            }

            BasicBlock current = null;
            for (int i = 0; i < IsLeader.Length; i++)
            {
                if (IsLeader[i])
                {
                    // New basic block.
                    current = new BasicBlock();

                    // Register in the dictionary.
                    LookupByLeader[func.InstructionList[i]] = current;

                    // Add to the list (used to preserve original order).
                    BlockList.Add(current);
                }

                current.AddInstruction(func.InstructionList[i]);
            }

            current = null;
            for (int i = 0; i < IsLeader.Length; i++)
            {
                if (IsLeader[i])
                {
                    // Make sure `current` is not the first basic block.
                    if (current != null)
                    {
                        EndBasicBlock(current, func.InstructionList[i]);
                    }

                    current = LookupByLeader[func.InstructionList[i]];
                }
            }

            // Finish up the last block.
            EndBasicBlock(current, null);
        }

        private void EndBasicBlock(BasicBlock block, IL.Instruction next)
        {
            // Calculate successors.
            IL.Instruction instr = block.LeavingInstruction;

            if (!(instr is IL.UnconditionalJumpInstruction) && next != null)
            {
                block.AddSuccessor(LookupByLeader[next]);
            }

            if (instr is IL.ConditionalJumpInstruction condJump)
            {
                block.AddSuccessor(LookupByLeader[condJump.Target]);
            }
            else if (instr is IL.UnconditionalJumpInstruction uncondJump)
            {
                block.AddSuccessor(LookupByLeader[uncondJump.Target]);
            }

            // Build DAG.
            block.Build();
        }

        public void Optimize()
        {
            OptimizeBasicBlocks();
        }

        private void OptimizeBasicBlocks()
        {
            // Optimize all basic blocks one by one.
            foreach (BasicBlock block in LookupByLeader.Values)
            {
                block.Optimize();
            }
        }

        public List<IL.Instruction> ReassembleInstructions()
        {
            List<IL.Instruction> list = new List<IL.Instruction>();
            foreach (BasicBlock block in BlockList)
            {
                list.AddRange(block.InstructionList);
            }
            return list;
        }

        public void Print()
        {
            foreach (BasicBlock block in LookupByLeader.Values)
            {
                block.Print();
                Console.WriteLine();
            }
        }

        public void PrintDAGs()
        {
            foreach (BasicBlock block in LookupByLeader.Values)
            {
                block.PrintDAG();
                Console.WriteLine();
            }
        }
    }
}
