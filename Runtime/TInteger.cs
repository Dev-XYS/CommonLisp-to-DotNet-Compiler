using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class TInteger : IType
    {
        public int Value { get; set; }
        public TInteger(int x)
        {
            Value = x;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
