using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Optimization.ControlFlow
{
    class BasicBlock
    {
        public List<IL.Instruction> InstructionList { get; private set; }

        public List<BasicBlock> Successor { get; }

        public IL.Instruction Leader { get => InstructionList[0]; }

        public IL.Instruction LeavingInstruction { get => InstructionList[InstructionList.Count - 1]; }

        public Dictionary<IL.Variable, Node> LastDef { get; set; }

        private DAG DAG { get; set; }

        // The following members are used for data flow analysis.
        public LivenessAnalysis.Info LA_Info { get; set; }

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
            DAG = BuildDAG();
        }

        private DAG BuildDAG()
        {
            DAG graph = new DAG();

            // Latest definition of a variable.
            LastDef = new Dictionary<IL.Variable, Node>();

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

        public void ConservativeMark()
        {
            foreach (Node node in LastDef.Values)
            {
                node.Essential = true;
            }
        }

        public void GlobalMark()
        {
            foreach (var pair in LastDef)
            {
                // Mark all alive variables as essential.
                if (LA_Info.Out.Contains(pair.Key))
                {
                    pair.Value.Essential = true;
                }
                // Mark all closure variables as essential.
                else if (Variable.NonLocalVariables.Contains(pair.Key))
                {
                    pair.Value.Essential = true;
                }
            }
        }

        // Optimize, assuming all variables are alive at the end of the block.
        // (i.e. No global analysis.)
        public void OptimizeLocally()
        {
            bool changed;

            // Iterate, until unchanged.
            do
            {
                // Build the DAG.
                Build();

                // Optimize the DAG.
                if (LA_Info == null)
                {
                    // If `LA_Info` == null, there is no global information,
                    // so we need to optimize conservatively.
                    ConservativeMark();
                }
                else
                {
                    // `LA_Info` presents, make using global information.
                    GlobalMark();
                }
                changed = DAG.Optimize();
                InstructionList = DAG.RewriteInstructions();
            } while (changed);
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
            DAG.Print();
        }
    }
}
