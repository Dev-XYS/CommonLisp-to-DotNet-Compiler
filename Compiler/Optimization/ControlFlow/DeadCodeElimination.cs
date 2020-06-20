using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization.ControlFlow
{
    static class DeadCodeElimination
    {
        public static bool Optimize(List<IL.Instruction> list, HashSet<IL.Variable> aliveVars)
        {
            bool changed = false;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                IL.Instruction instr = list[i];

                if ((instr is IL.FunctionInstruction || instr is IL.MoveInstruction) && !aliveVars.Contains(instr.DefinedVariable) && !Variable.NonLocalVariables.Contains(instr.DefinedVariable))
                {
                    // The variable is not alive, we can safely delete the instruction.
                    // Also, the variable must not be in a closure.
                    list.RemoveAt(i);
                    changed = true;
                }
                else
                {
                    // Update alive variables info.
                    aliveVars.UnionWith(instr.UsedVariables);
                }
            }

            return changed;
        }
    }
}
