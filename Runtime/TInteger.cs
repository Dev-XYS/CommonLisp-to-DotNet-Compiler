using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    class TInteger : IType
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
    }
}
