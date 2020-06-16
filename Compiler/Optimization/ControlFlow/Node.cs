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

        public Node(IL.Instruction instr)
        {
            Instruction = instr;
            Dependencies = new HashSet<Node>();
        }

        public void AddDependency(Node node)
        {
            Dependencies.Add(node);
        }

        public override string ToString()
        {
            return Instruction.ToString();
        }
    }
}
