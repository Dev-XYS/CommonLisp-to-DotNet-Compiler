using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    using I = Instructions;

    partial class Function : Class
    {
        public override string Name { get; }

        public override string CtorArgumentList
        {
            get => GetCtorArgumentList();
        }

        public Program Program { get; }
        private IL.Function ILFunction { get; }

        public List<Instruction> CtorInstructionList { get; }
        public List<Instruction> InstructionList { get; }

        private List<EnvironmentMember> EnvList { get; }
        public Dictionary<IL.Environment, EnvironmentMember> EnvMap { get; }

        public int VarNo { get; set; }
        public Dictionary<IL.Variable, int> VarMap { get; }

        public Function(Program prog, IL.Function func)
        {
            Name = "func" + func.GetHashCode().ToString();
            Program = prog;
            ILFunction = func;

            CtorInstructionList = new List<Instruction>();
            InstructionList = new List<Instruction>();
            EnvList = new List<EnvironmentMember>();
            EnvMap = new Dictionary<IL.Environment, EnvironmentMember>();
            VarMap = new Dictionary<IL.Variable, int>();

            foreach (IL.Environment env in func.EnvList)
            {
                EnvironmentMember m = new EnvironmentMember(this, env);
                EnvList.Add(m);
                EnvMap[env] = m;
            }
        }

        public void Generate()
        {
            GenCtor();
            GenBody(ILFunction);
        }

        private void CtorGen(Instruction instr)
        {
            CtorInstructionList.Add(instr);
        }

        private void GenCtor()
        {
            CtorGen(new I.LoadArgument { ArgNo = 0 });
            CtorGen(new I.CallObjectCtor { });

            for (int i = 1; i < EnvList.Count; i++)
            {
                CtorGen(new I.LoadArgument { ArgNo = 0 });
                CtorGen(new I.LoadArgument { ArgNo = i });
                CtorGen(new I.StoreField { Field = EnvList[i] });
            }

            CtorGen(new I.Return { });
        }

        private void GenBody(IL.Function func)
        {
            // create current environment
            Gen(new I.LoadArgument { ArgNo = 0 });
            Gen(new I.NewObject { Type = EnvList[0].Environment });
            Gen(new I.StoreField { Field = EnvList[0] });

            // copy parameters to the environment
            int no = 0;
            foreach (IL.Variable var in func.Parameters)
            {
                Gen(new I.LoadArgument { ArgNo = 0 });
                Gen(new I.LoadField { Field = EnvList[0] });
                Gen(new I.LoadArgument { ArgNo = 1 });
                Gen(new I.LoadInt { Value = no++ });
                Gen(new I.LoadElement { });
                Gen(new I.StoreField { Field = EnvList[0].Environment.VarMap[var] });
            }

            foreach (IL.IInstruction instr in func.InstructionList)
            {
                Gen(new I.Comment { Content = instr.ToString() });
                GenInstruction(instr);
            }
        }

        private string GetCtorArgumentList()
        {
            string r = "";
            if (EnvList.Count > 1)
            {
                r = string.Format("class {0} @{1}", EnvList[1].Type, EnvList[1].Name);
            }
            for (int i = 2; i < EnvList.Count; i++)
            {
                r += string.Format(", class {0} @{1}", EnvList[i].Type, EnvList[i].Name);
            }
            return r;
        }

        public void Emit()
        {
            Emitter.Emit(".class private auto ansi beforefieldinit {0} extends [System.Runtime]System.Object implements [Runtime]Runtime.IType", Name);
            Emitter.BeginBlock();

            foreach (EnvironmentMember m in EnvList)
            {
                Emitter.Emit(".field private class {0} {1}", m.Environment.Name, m.Name);
            }

            Emitter.Emit(".method public hidebysig specialname rtspecialname instance void .ctor({0}) cil managed", GetCtorArgumentList());
            Emitter.BeginBlock();
            foreach (Instruction instr in CtorInstructionList)
            {
                instr.Emit();
            }
            Emitter.EndBlock();

            Emitter.Emit(".method public hidebysig newslot virtual final instance class [Runtime]Runtime.IType Invoke(class [Runtime]Runtime.IType[] args) cil managed");
            Emitter.BeginBlock();
            Emitter.Emit(".locals init (class [Runtime]Runtime.IType Temp, class [Runtime]Runtime.IType[] Args)");
            foreach (Instruction instr in InstructionList)
            {
                instr.Emit();
            }
            Emitter.EndBlock();

            Emitter.EndBlock();
        }
    }
}
