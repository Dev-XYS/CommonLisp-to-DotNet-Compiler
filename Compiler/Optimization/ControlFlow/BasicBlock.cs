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

        public DAG BuildDAG()
        {
            DAG graph = new DAG();

            // Latest definition of a variable.
            Dictionary<IL.Variable, Node> LastDef = new Dictionary<IL.Variable, Node>();

            Node previous = null;

            foreach (IL.Instruction instr in InstructionList)
            {
                // Create a DAG node for the instruction.
                Node node = new Node(instr);

                foreach (IL.Variable var in instr.UsedVariables)
                {
                    // Add dependency.
                    Node dependency = LastDef.GetValueOrDefault(var);
                    if (dependency != null)
                    {
                        node.AddDependency(dependency);
                    }
                }

                // Update defined variable.
                IL.Variable def = instr.DefinedVariable;
                if (def != null)
                {
                    LastDef[def] = node;
                    Console.WriteLine("=> updated {0}", def);
                }

                // Every node depends on its previous node (to preserve order).
                if (previous != null)
                {
                    node.AddDependency(previous);
                }
                previous = node;

                graph.AddNode(node);
            }

            return graph;
        }
    }
}
