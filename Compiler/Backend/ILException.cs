using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Backend
{
    class ILException : Exception
    {
        public ILException(string message) : base(message)
        {
        }
    }
}
