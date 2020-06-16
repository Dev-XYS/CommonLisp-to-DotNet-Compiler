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

        private DAG Graph { get; set; }

        public BasicBlock()
        {
            InstructionList = new List<IL.Instruction>();
            Successor = new List<BasicBlock>();
        }

        public void AddInstruction(IL.Instruction instr)
        {
            InstructionList.Add(instr);
        }

        public void AddSuccessor(BasicBlock block)
        {
            Successor.Add(block);
        }

        public void Build()
        {
            Graph = BuildDAG();
        }

        private DAG BuildDAG()
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

            // Currently, assume that all variables are alive.
            foreach (Node node in LastDef.Values)
            {
                node.Essential = true;
            }

            return graph;
        }

        public void Optimize()
        {
            Graph.Optimize();
        }

        public void Print()
        {
            Console.WriteLine("--- basic block {0} ---", GetHashCode());
            foreach (IL.Instruction instr in InstructionList)
            {
                Console.WriteLine(instr);
            }
            Console.WriteLine(">>> next block(s)");
            foreach (BasicBlock block in Successor)
            {
                Console.WriteLine(block.GetHashCode());
            }
            Console.WriteLine("<<<");
        }

        public void PrintDAG()
        {
            Console.WriteLine("--- DAG of basic block {0} ---", GetHashCode());
            Graph.Print();
        }
    }
}
