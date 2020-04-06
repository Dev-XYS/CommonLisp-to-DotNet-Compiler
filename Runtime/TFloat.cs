using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class TFloat : INumber
    {
        public double Value { get; set; }
        public TFloat(double x)
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
        public TFloat ToFloat()
        {
            return this;
        }
        public TInteger ToInt()
        {
            return new TInteger(Convert.ToInt32(Value));
        }
    }
}
