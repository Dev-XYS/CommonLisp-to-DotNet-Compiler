using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    class Instruction
    {
        public IL.IInstruction ILInstruction { get; }

        private IL.Function ILFunction { get; }

        public bool Necessary { get; set; }

        public IL.Variable LocallyDefinedVariable
        {
            get
            {
                // Call, Func, Move define variables, others do not.
                IL.Variable variable = null;
                if (ILInstruction is IL.CallInstruction call)
                {
                    variable = call.Destination;
                }
                else if (ILInstruction is IL.FunctionInstruction func)
                {
                    variable = func.Destination;
                }
                else if (ILInstruction is IL.MoveInstruction move)
                {
                    variable = move.Destination;
                }

                // No defined variable.
                if (variable == null)
                {
                    return null;
                }

                // We only look for local variables.
                // Check if the variable is contained in the innermost environment.
                if (variable.Env == ILFunction.EnvList[0])
                {
                    return variable;
                }
                else
                {
                    return null;
                }
            }
        }

        public IL.Variable LocallyUsedVariable
        {
            get
            {
                // Most instructions use variable.
                IL.IEntity entity = null;
                if (ILInstruction is IL.CallInstruction call)
                {
                    entity = call.Function;
                }
                else if (ILInstruction is IL.ConditionalJumpInstruction condJump)
                {
                    entity = condJump.TestVariable;
                }
                else if (ILInstruction is IL.MoveInstruction move)
                {
                    entity = move.Source;
                }
                else if (ILInstruction is IL.ReturnInstruction ret)
                {
                    entity = ret.Value;
                }

                // No defined variable.
                if (entity == null)
                {
                    return null;
                }

                // Not a variable. (Immediate numbers, etc.)
                if (!(entity is IL.Variable))
                {
                    return null;
                }

                // We only look for local variables.
                // Check if the variable is contained in the innermost environment.
                if ((entity as IL.Variable).Env == ILFunction.EnvList[0])
                {
                    return entity as IL.Variable;
                }
                else
                {
                    return null;
                }
            }
        }

        public Instruction(IL.IInstruction instr, IL.Function func)
        {
            ILInstruction = instr;
            ILFunction = func;
            if (instr is IL.ReturnInstruction || instr is IL.CallInstruction)
            {
                Necessary = true;
            }
        }
    }
}
