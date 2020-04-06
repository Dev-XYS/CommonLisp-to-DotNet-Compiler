using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string format, params object[] args) : base(string.Format(format, args))
        {
        }
    }
}
