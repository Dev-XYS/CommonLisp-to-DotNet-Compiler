using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    static class PeepHole
    {
        static private IL.Program Program;

        static public IL.Program Optimize(IL.Program program)
        {
            Program = program;
            OptimizeAllFunctions();
            return Program;
        }

        static private void OptimizeAllFunctions()
        {
            foreach (IL.Function func in Program.FunctionList)
            {
                ControlSequence seq = new ControlSequence(func.InstructionList, func);
                Dictionary<IL.Variable, Instruction> LastDef = new Dictionary<IL.Variable, Instruction>();
                foreach (Instruction instr in seq.Instructions)
                {
                    IL.Variable var;

                    // Find the last instruction which defines the used variable,
                    // and mark it as necessary.
                    var = instr.LocallyUsedVariable;
                    if (var != null)
                    {
                        Instruction lastDefInstr = LastDef[var];
                        if (lastDefInstr != null)
                        {
                            lastDefInstr.Necessary = true;
                        }
                    }

                    // Update the last definition instruction.
                    var = instr.LocallyDefinedVariable;
                    if (var != null)
                    {
                        LastDef[var] = instr;
                    }
                }

                // Reconstruct the instruction list.
                func.InstructionList.Clear();
                foreach (Instruction instr in seq.Instructions)
                {
                    if (instr.Necessary)
                    {
                        func.InstructionList.Add(instr.ILInstruction);
                    }
                }
            }
        }
    }
}
