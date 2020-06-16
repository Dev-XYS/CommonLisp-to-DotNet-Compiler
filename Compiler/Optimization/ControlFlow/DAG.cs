using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Optimization.ControlFlow
{
    class DAG
    {
        private List<Node> Nodes { get; }

        public DAG()
        {
            Nodes = new List<Node>();
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public void Optimize()
        {
            // Currently, assume that all node of 0 degree is alive.
            foreach (Node node in Nodes)
            {
                if (node.InDegree == 0)
                {
                    node.Essential = true;
                }
            }

            foreach (Node node in Nodes)
            {
                // The node must be a "root". (i.e. `InDegree` = 0)
                // The node must have not been optimized.
                if (node.InDegree == 0 && !node.Optimized)
                {
                    Optimize(node);
                }
            }
        }

        private void Optimize(Node node)
        {
            if (node.Optimized)
            {
                return;
            }

            // Optimize recursively.
            foreach (Node next in node.Dependencies)
            {
                Optimize(next);
            }

            IL.Instruction instr = node.Instruction;

            // Even if the instruction is not optimized, `Alternative` should be set.
            node.Alternative = node.Original;

            // Essential nodes cannot be removed.
            // (They are alive at the end of the basic block.)
            if (!node.Essential)
            {
                // Only move instruction may be removed.
                if (instr is IL.MoveInstruction move)
                {
                    // If the instruction has only one or zero dependency, it can be removed.
                    if (node.OutDegree == 1)
                    {
                        node.Removed = true;

                        Node dependency = node.Dependencies.First();

                        if (move.Source == dependency.Instruction.DefinedVariable)
                        {
                            // Pattern: b = a, c = b.
                            node.Alternative = dependency.Alternative;
                        }
                        else
                        {
                            node.Alternative = move.Source;
                        }
                    }
                    else if (node.OutDegree == 0)
                    {
                        node.Removed = true;
                        node.Alternative = move.Source;
                    }
                }
            }
        }

        public void Print()
        {
            foreach (Node node in Nodes)
            {
                Console.WriteLine(
                    "{0} {1} (depends on: {2}), essential = {3}, removed = {4}, original = {5}, alternative = {6}",
                    node.GetHashCode(),
                    node.Instruction,
                    string.Join(", ", node.Dependencies.ToList().ConvertAll((Node node) => node.GetHashCode())),
                    node.Essential,
                    node.Removed,
                    node.Original,
                    node.Alternative
                );
            }
        }
    }
}
