using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public interface IInputStream
    {
        public int ReadChar();
        public void UnReadChar();
    }
}
