using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Optimization
{
    static class TailRecursion
    {
        public static Dictionary<IL.Variable, IL.Function> FuncMap;

        public static Program Optimize(Program program)
        {
            Initialize(program);

            foreach (Function func in program.FunctionList)
            {
                OptimizeFunction(func);
            }

            return program;
        }

        class Node
        {
            public IL.Instruction Instruction { get; set; }

            public HashSet<Node> Next { get; }

            public Node(IL.Instruction instr)
            {
                Instruction = instr;
                Next = new HashSet<Node>();
            }
        }

        class Graph
        {
            public Node Entry { get; }

            public List<Node> NodeList { get; }

            public Graph(Function func)
            {
                NodeList = new List<Node>();

                Dictionary<IL.Instruction, Node> Map = new Dictionary<IL.Instruction, Node>();

                foreach (IL.Instruction instr in func.InstructionList)
                {
                    Node node = new Node(instr);
                    NodeList.Add(node);
                    Map[instr] = node;
                }

                // Calculate successors.
                for (int i = 0; i < func.InstructionList.Count; i++)
                {
                    IL.Instruction instr = func.InstructionList[i];
                    if (instr is IL.UnconditionalJumpInstruction uncondJump)
                    {
                        Map[instr].Next.Add(Map[uncondJump.Target]);
                    }
                    else
                    {
                        if (i + 1 < func.InstructionList.Count)
                        {
                            Map[instr].Next.Add(Map[func.InstructionList[i + 1]]);
                        }

                        if (instr is IL.ConditionalJumpInstruction condJump)
                        {
                            Map[instr].Next.Add(Map[condJump.Target]);
                        }
                    }
                }

                // The entry is the first instruction.
                Entry = Map[func.InstructionList[0]];
            }
        }

        private static void Initialize(Program program)
        {
            FuncMap = new Dictionary<IL.Variable, IL.Function>();

            foreach (Function func in program.FunctionList)
            {
                foreach (IL.Instruction instr in func.InstructionList)
                {
                    if (instr.DefinedVariable == null)
                    {
                        continue;
                    }
                    if (FuncMap.ContainsKey(instr.DefinedVariable))
                    {
                        FuncMap[instr.DefinedVariable] = null;
                    }
                    else if (instr is IL.FunctionInstruction funcInstr)
                    {
                        // Local variables cannot be a dedicated function.
                        if (!(instr.DefinedVariable is LocalVariable))
                        {
                            FuncMap[instr.DefinedVariable] = funcInstr.Function;
                        }
                    }
                }
            }
        }

        private static void OptimizeFunction(Function func)
        {
            Graph graph = new Graph(func);

            // Optimize tail calls.
            foreach (Node node in graph.NodeList)
            {
                if (node.Instruction is IL.CallInstruction call)
                {
                    if (IsRecursiveCall(call, func) && IsTailRecursion(node))
                    {
                        node.Instruction = GenTailRecursiveCall(call);
                    }
                }
            }

            // Rewrite instructions.
            func.InstructionList.Clear();
            foreach (Node node in graph.NodeList)
            {
                func.InstructionList.Add(node.Instruction);
            }
        }

        private static bool IsRecursiveCall(IL.CallInstruction call, Function func)
        {
            // Check if the call is recursive.
            // (Need a better method.)
            if (call.Function is IL.Variable varFunc)
            {
                if (FuncMap.GetValueOrDefault(varFunc) == func.ILFunction)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsTailRecursion(Node node)
        {
            return IsTailRecursion(node.Next.First(), (node.Instruction as IL.CallInstruction).DefinedVariable);
        }

        private static bool IsTailRecursion(Node node, IL.Variable var)
        {
            // If the instruction is return, and the returned value is the result of the recursion,
            // we can conclude that it is a tail recursion.
            if (node.Instruction is IL.ReturnInstruction ret)
            {
                if (ret.Value == var)
                {
                    return true;
                }
            }

            // If the current instruction is a label or unconditional jump,
            // find the path recursively.
            if (node.Instruction is IL.UnconditionalJumpInstruction || node.Instruction is IL.Label)
            {
                return IsTailRecursion(node.Next.First(), var);
            }

            return false;
        }

        private static IL.Instruction GenTailRecursiveCall(IL.CallInstruction call)
        {
            return new TailCallInstruction(call.Parameters);
        }
    }
}
