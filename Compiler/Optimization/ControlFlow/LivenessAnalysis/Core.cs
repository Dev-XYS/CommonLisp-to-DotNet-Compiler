using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Optimization.ControlFlow.LivenessAnalysis
{
    static class Core
    {
        public static void Analyze(Graph graph)
        {
            Initialize(graph);

            // Run fixed point algorithm.
            FixedPoint(graph);
        }

        private static void Initialize(Graph graph)
        {
            // Calculate use and def for all basic blocks.
            // Meanwhile, initialize all `In` and `Out` to empty set.
            foreach (BasicBlock block in graph.BlockList)
            {
                LivenessAnalysis.Info info = new Info();
                foreach (IL.Instruction instr in block.InstructionList)
                {
                    info.AddDef(instr.DefinedVariable);
                    info.AddUse(instr.UsedVariables);
                }
                block.LA_Info = info;
            }

            // Boundary: `Exit.In` = all closure variables.
            // (The first environment is the local environment, skipped.)
            graph.Exit.LA_Info = new Info();
            for (int i = 1; i < graph.Function.EnvList.Count; i++)
            {
                IL.Environment env = graph.Function.EnvList[i];
                graph.Exit.LA_Info.In.UnionWith(env.VariableList);
            }
        }

        private static void FixedPoint(Graph graph)
        {
            bool Changed;
            do
            {
                Changed = false;

                foreach (BasicBlock block in graph.BlockList)
                {
                    // Calculate `Out` (the union of `In` of successors).
                    HashSet<IL.Variable> Out = new HashSet<IL.Variable>();
                    foreach (BasicBlock successor in block.Successor)
                    {
                        Out.UnionWith(successor.LA_Info.In);
                    }
                    if (!block.LA_Info.Out.SetEquals(Out))
                    {
                        block.LA_Info.Out = Out;
                        Changed = true;
                    }

                    // Calculate `In` (using transfer function: `Use` union ).
                    HashSet<IL.Variable> In = TransferFunction(Out, block.LA_Info.Def, block.LA_Info.Use);
                    if (!block.LA_Info.In.SetEquals(In))
                    {
                        block.LA_Info.In = In;
                        Changed = true;
                    }
                }
            } while (Changed);
        }

        private static HashSet<IL.Variable> TransferFunction(HashSet<IL.Variable> Out, HashSet<IL.Variable> Def, HashSet<IL.Variable> Use)
        {
            return Use.Union(Out.Except(Def)).ToHashSet();
        }

        private static void PrintLivenessInfo(Graph graph)
        {
            foreach (BasicBlock block in graph.BlockList)
            {
                Console.WriteLine("--- liveness info at the end of block {0} ---", block.GetHashCode());
                foreach (IL.Variable var in block.LA_Info.Out)
                {
                    Console.WriteLine(var);
                }
                Console.WriteLine();
            }
        }
    }
}
