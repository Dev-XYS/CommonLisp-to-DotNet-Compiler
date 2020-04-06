using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class TBool : IType
    {
        private enum Bool { NIL, T };
        private Bool Value;
        private TBool(Bool b)
        {
            Value = b;
        }
        public override string ToString()
        {
            if (Value == Bool.T)
                return "T";
            else return "NIL";
        }
        public static TBool T()
        {
            return new TBool(Bool.T);
        }
        public static TBool NIL()
        {
            return new TBool(Bool.NIL);
        }

        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
