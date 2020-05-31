using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    using I = Instructions;

    partial class Function
    {
        public void GenInstruction(IL.Instruction instr)
        {
            if (instr is IL.CallInstruction)
            {
                GenCall(instr as IL.CallInstruction);
            }
            else if (instr is IL.FunctionInstruction)
            {
                GenFunction(instr as IL.FunctionInstruction);
            }
            else if (instr is IL.MoveInstruction)
            {
                GenMove(instr as IL.MoveInstruction);
            }
            else if (instr is IL.ReturnInstruction)
            {
                GenReturn(instr as IL.ReturnInstruction);
            }
            else if (instr is IL.Label)
            {
                GenLabel(instr as IL.Label);
            }
            else if (instr is IL.UnconditionalJumpInstruction)
            {
                GenUnconditionalJump(instr as IL.UnconditionalJumpInstruction);
            }
            else if (instr is IL.ConditionalJumpInstruction)
            {
                GenConditionalJump(instr as IL.ConditionalJumpInstruction);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void Gen(Instruction instr)
        {
            InstructionList.Add(instr);
        }

        private void GenLoadLoc(Optimization.LocalVariable loc)
        {
            Gen(new I.Load { Loc = loc.LocSlot });
        }

        private void GenLoadVariable(IL.Variable var)
        {
            if (var.Env != null)
            {
                // Load environment
                Gen(new I.LoadArgument { ArgNo = 0 });
                Gen(new I.LoadField { Field = EnvMap[var.Env] });
                Gen(new I.LoadField { Field = EnvMap[var.Env].Environment.VarMap[var] });
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void GenLoadUnresolvedObject(IL.UnresolvedObject obj)
        {
            // Currently, all unresolved objects are lisp library functions.
            // Instantiate the function.
            Gen(new I.Text { Content = string.Format("newobj instance void [Library]{0}::.ctor()", obj.Name) });
        }

        private void GenLoadConst(Runtime.IType c)
        {
            // Every constant can be null.
            if (c == null)
            {
                Gen(new I.LoadNull { });
            }
            // The constant is data.
            else if (c is Runtime.IDataType)
            {
                Program.Const.Register(c as Runtime.IDataType);
                Gen(new I.LoadStaticField { StaticClass = "Constants", Name = "const" + c.GetHashCode() });
            }
            // The constant is a function.
            // -> Need a better way to determine if a constant is a function.
            else if (c.GetType().Namespace.StartsWith("Runtime.Function"))
            {
                Gen(new I.NewObject { Type = new RuntimeObject { Type = c.GetType() } });
            }
            // It seems that there aren't other types of constant.
            else
            {
                // This should not happen.
                throw new NotImplementedException();
            }
        }

        private void GenLoadEntity(IL.IEntity e)
        {
            if (e == null)
            {
                // This should not happen.
                throw new NotImplementedException();
            }
            else if (e is Optimization.LocalVariable loc)
            {
                // The entity is a (purely) local variable (used for optimization).
                GenLoadLoc(loc);
            }
            else if (e is IL.Variable var)
            {
                // The entity is a variable.
                GenLoadVariable(var);
            }
            else if (e is IL.UnresolvedObject obj)
            {
                // The entity represents an unresolved name, i.e. a library object.
                GenLoadUnresolvedObject(obj);
            }
            else
            {
                // The entity is an immediate number.
                IL.ImmediateNumber imm = e as IL.ImmediateNumber;
                GenLoadConst(imm.Imm);
            }
        }

        private void GenStoreTemp(IL.Variable var)
        {
            if (var.Env != null)
            {
                Gen(new I.LoadArgument { ArgNo = 0 });
                Gen(new I.LoadField { Field = EnvMap[var.Env] });
                Gen(new I.Load { Loc = 0 });
                Gen(new I.StoreField { Field = EnvMap[var.Env].Environment.VarMap[var] });
            }
            else if (var is Optimization.LocalVariable loc)
            {
                Gen(new I.Load { Loc = 0 });
                Gen(new I.Store { Loc = loc.LocSlot });
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void GenCall(IL.CallInstruction instr)
        {
            GenLoadEntity(instr.Function);
            Gen(new I.LoadInt { Value = instr.Parameters.Count });
            Gen(new I.NewArray { Type = "[Runtime]Runtime.IType" });
            Gen(new I.Store { Loc = 1 });
            int no = 0;
            foreach (IL.IEntity e in instr.Parameters)
            {
                Gen(new I.Load { Loc = 1 });
                Gen(new I.LoadInt { Value = no++ });
                GenLoadEntity(e);
                Gen(new I.StoreElement { });
            }
            Gen(new I.Load { Loc = 1 });
            Gen(new I.CallVirtual { });
            Gen(new I.Store { Loc = 0 });
            GenStoreTemp(instr.Destination);
        }

        private void GenFunction(IL.FunctionInstruction instr)
        {
            foreach (EnvironmentMember env in EnvList)
            {
                Gen(new I.LoadArgument { ArgNo = 0 });
                Gen(new I.LoadField { Field = env });
            }
            Gen(new I.NewObject { Type = Program.FuncMap[instr.Function] });
            Gen(new I.Store { Loc = 0 });
            GenStoreTemp(instr.Destination);
        }

        private void GenMove(IL.MoveInstruction instr)
        {
            GenLoadEntity(instr.Source);
            Gen(new I.Store { Loc = 0 });
            GenStoreTemp(instr.Destination);
        }

        private void GenReturn(IL.ReturnInstruction instr)
        {
            GenLoadEntity(instr.Value);
            Gen(new I.LoadArgument { ArgNo = 0 });
            Gen(new I.Load { Loc = 2 });
            Gen(new I.StoreField { Field = EnvList[0] });
            Gen(new I.Return { });
        }

        private void GenLabel(IL.Label label)
        {
            Gen(new I.Label { ILLabel = label });
        }

        private void GenUnconditionalJump(IL.UnconditionalJumpInstruction instr)
        {
            Gen(new I.Branch { Target = instr.Target });
        }

        private void GenConditionalJump(IL.ConditionalJumpInstruction instr)
        {
            GenLoadVariable(instr.TestVariable);
            if (instr.Condition)
            {
                Gen(new I.BranchTrue { Target = instr.Target });
            }
            else
            {
                Gen(new I.BranchNull { Target = instr.Target });
            }
        }
    }
}
