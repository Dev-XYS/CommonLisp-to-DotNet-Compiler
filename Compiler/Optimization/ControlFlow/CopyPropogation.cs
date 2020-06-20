using Compiler.IL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization.ControlFlow
{
    static class CopyPropogation
    {
        public static void Optimize(Program program)
        {
            foreach (Function func in program.FunctionList)
            {
                Optimize(func.InstructionList);
            }
        }

        public static void Optimize(List<IL.Instruction> list)
        {
            // Store copies of variables.
            Dictionary<IL.Variable, IEntity> copiesMap = new Dictionary<IL.Variable, IEntity>();

            foreach (IL.Instruction instr in list)
            {
                foreach (IL.Variable var in instr.UsedVariables)
                {
                    IEntity copy = copiesMap.GetValueOrDefault(var);

                    // No copy of `var`.
                    if (copy == null)
                    {
                        continue;
                    }

                    // Copy of `var` exists, use the earliest.
                    instr.ReplaceUsedValue(var, copy);
                }

                // Kill the copies of defined variable.
                if (instr.DefinedVariable != null)
                {
                    foreach (IL.Variable var in copiesMap.Keys)
                    {
                        if (copiesMap[var] == instr.DefinedVariable)
                        {
                            copiesMap.Remove(var);
                        }
                    }
                }

                // Store new copies.
                if (instr is IL.MoveInstruction move)
                {
                    copiesMap[move.Destination] = move.Source;
                }
                else if (instr.DefinedVariable != null)
                {
                    copiesMap.Remove(instr.DefinedVariable);
                }
            }
        }
    }
}
