using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public interface IOutputStream
    {
        public void PutChar(char ch);
        public void PutS(string s);
    }
}
