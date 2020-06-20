using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Compiler.Optimization.ControlFlow
{
    class Node
    {
        public IL.Instruction Instruction { get; }

        public HashSet<Node> Dependencies { get; }

        public int OutDegree { get => Dependencies.Count; }

        public int InDegree { get; private set; }

        public bool Removed { get; set; }

        public IL.IEntity Original { get => Instruction.DefinedVariable; }

        public IL.IEntity Alternative { get; set; }

        // Mark that if the node is alive at the end of the block.
        public bool Essential { get; set; }

        // Mark that if the node has been optimized.
        public bool Optimized { get; set; }

        // Mark that if the node has been rewritten.
        public bool Rewritten { get; set; }

        public Node(IL.Instruction instr)
        {
            Instruction = instr;
            Dependencies = new HashSet<Node>();
        }

        public void AddDependency(Node node)
        {
            if (Dependencies.Add(node))
            {
                node.InDegree++;
            }
        }

        public override string ToString()
        {
            return Instruction.ToString();
        }
    }
}
