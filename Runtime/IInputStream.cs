using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    interface IInputStream
    {
        public int ReadChar();
        public void UnReadChar();
    }
}
