using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    class TFloat : IType 
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
    }
}
