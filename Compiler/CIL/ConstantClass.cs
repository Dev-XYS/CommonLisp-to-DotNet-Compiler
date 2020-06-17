using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    using I = Instructions;

    /// <summary>
    /// Represents the collection of constants used by a program.
    /// </summary>
    class ConstantClass
    {
        private HashSet<Runtime.IDataType> Constants { get; }

        private List<Instruction> CtorInstructionList { get; }

        public ConstantClass()
        {
            Constants = new HashSet<Runtime.IDataType>();
            CtorInstructionList = new List<Instruction>();
        }

        public void Register(Runtime.IDataType data)
        {
            Constants.Add(data);
        }

        public void Generate()
        {
            foreach (Runtime.IDataType data in Constants)
            {
                GenLoadField(data);
            }
            CtorGen(new I.Return { });
        }

        private void GenLoadField(Runtime.IDataType data)
        {
            GenLoadConstant(data);
            CtorGen(new I.StoreStaticField { Name = "const" + data.GetHashCode() });
        }

        private void GenLoadConstant(Runtime.IDataType data)
        {
            if (data == null)
            {
                CtorGen(new I.LoadNull { });
            }
            else if (data is Runtime.Cons cons)
            {
                GenLoadConstant(cons.car as Runtime.IDataType);
                GenLoadConstant(cons.cdr as Runtime.IDataType);
                CtorGen(new I.NewObject { Type = new RuntimeObject { Type = typeof(Runtime.Cons) } });
            }
            else if (data is Runtime.T)
            {
                CtorGen(new I.NewObject { Type = new RuntimeObject { Type = typeof(Runtime.T) } });
            }
            else if (data is Runtime.TInteger integer)
            {
                CtorGen(new I.LoadInt { Value = integer.Value });
                CtorGen(new I.NewObject { Type = new RuntimeObject { Type = typeof(Runtime.TInteger) } });
            }
            else if (data is Runtime.Symbol sym)
            {
                CtorGen(new I.LoadString { Value = sym.Name });
                CtorGen(new I.Call { FullName = "[Runtime]Runtime.Symbol [Runtime]Runtime.Symbol::FindOrCreate(string)" });
            }
            else if (data is Runtime.TString str)
            {
                CtorGen(new I.LoadString { Value = str.Value });
                CtorGen(new I.NewObject { Type = new RuntimeObject { Type = typeof(Runtime.TString) } });
            }
            else
            {
                // Other types are not implemented yet.
                throw new NotImplementedException();
            }
        }

        private void CtorGen(Instruction instr)
        {
            CtorInstructionList.Add(instr);
        }

        public void Emit()
        {
            Emitter.Emit(".class private abstract auto ansi sealed beforefieldinit Constants extends [System.Runtime]System.Object");
            Emitter.BeginBlock();

            // Program constants.
            foreach (Runtime.IDataType data in Constants)
            {
                Emitter.Emit(".field public static initonly class [Runtime]Runtime.IType const{0}", data.GetHashCode());
            }

            // Main function of the library (contains the root environment of the library).
            Emitter.Emit(".field public static initonly class [Library]LibMain LibMain");

            Emitter.Emit(".method private hidebysig specialname rtspecialname static void .cctor() cil managed");
            Emitter.BeginBlock();
            foreach (Instruction instr in CtorInstructionList)
            {
                instr.Emit();
            }
            Emitter.EndBlock();
            Emitter.EndBlock();
        }
    }
}
