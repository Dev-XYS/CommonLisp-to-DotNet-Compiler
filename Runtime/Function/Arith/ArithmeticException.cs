using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function.Arith
{
    class ArithmeticException : RuntimeException
    {
        public ArithmeticException(string s, params object[] vs) : base(s, vs) { }

    }
}
