using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class TInteger : INumber
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
        public TInteger ToInt()
        {
            return this;
        }
        public TFloat ToFloat()
        {
            return new TFloat(Convert.ToDouble(Value));
        }
    }
}
