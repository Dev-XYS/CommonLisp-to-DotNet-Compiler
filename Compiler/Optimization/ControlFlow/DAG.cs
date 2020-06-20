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

        public bool Optimize()
        {
            bool changed = false;
            foreach (Node node in Nodes)
            {
                // The node must be a "root". (i.e. `InDegree` = 0)
                // The node must have not been optimized.
                if (node.InDegree == 0 && !node.Optimized)
                {
                    changed |= Optimize(node);
                }
            }
            return changed;
        }

        private bool Optimize(Node node)
        {
            // Check if the node has been visited.
            if (node.Optimized)
            {
                return false;
            }

            // Mark if changes are made.
            bool changed = false;

            // Optimize recursively.
            foreach (Node next in node.Dependencies)
            {
                changed |= Optimize(next);
            }

            // Even if the instruction is not optimized, `Alternative` should be set.
            node.Alternative = node.Original;

            // Essential nodes cannot be removed.
            // (They are alive at the end of the basic block, or are function calls, etc.)
            if (node.Essential)
            {
                return changed;
            }

            // The current instruction.
            IL.Instruction instr = node.Instruction;

            // Only move instruction may be removed.
            if (instr is IL.MoveInstruction move)
            {
                // If the instruction dependency forms a chain, it can be removed.
                if (node.OutDegree == 1 && node.InDegree == 1)
                {
                    node.Removed = true;

                    changed = true;

                    Node dependency = node.Dependencies.First();

                    if (move.Source == dependency.Instruction.DefinedVariable)
                    {
                        // Pattern: b = a, c = b.
                        node.Alternative = dependency.Alternative;
                    }
                    else
                    {
                        // Unnecessary code?
                        node.Alternative = move.Source;
                    }
                }
                else if (node.OutDegree == 0)
                {
                    node.Removed = true;
                    node.Alternative = move.Source;
                }
            }

            return changed;
        }

        public List<IL.Instruction> RewriteInstructions()
        {
            List<IL.Instruction> list = new List<IL.Instruction>();

            // Traverse the DAG by topological order.
            // Rewrite all nodes which are not removed.
            foreach (Node node in Nodes)
            {
                // The node must be a "root". (i.e. `InDegree` = 0)
                // The node must have not been optimized.
                if (node.InDegree == 0 && !node.Rewritten)
                {
                    Traverse(node, list);
                }
            }

            return list;
        }

        private void Traverse(Node node, List<IL.Instruction> list)
        {
            // Skip rewritten nodes.
            if (node.Rewritten == true)
            {
                return;
            }
            node.Rewritten = true;

            // Recursively traverse each child.
            foreach (Node child in node.Dependencies)
            {
                Traverse(child, list);
            }

            // Rewrite the current instruction.
            if (!node.Removed)
            {
                foreach (Node child in node.Dependencies)
                {
                    node.Instruction.ReplaceUsedValue(child.Original, child.Alternative);
                }
                list.Add(node.Instruction);
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
