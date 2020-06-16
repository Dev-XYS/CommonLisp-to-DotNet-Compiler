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

        public void Print()
        {
            foreach (Node node in Nodes)
            {
                Console.WriteLine(
                    "{0} {1} (depends on: {2})",
                    node.GetHashCode(),
                    node.Instruction,
                    string.Join(", ", node.Dependencies.ToList().ConvertAll((Node node) => node.GetHashCode()))
                );
            }
        }
    }
}
