using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class StandardOutput : IOutputStream
    {
        public void PutChar(char ch)
        {
            Console.Write(ch);
        }
        public void PutS(string s)
        {
            Console.Write(s);
        }
    }
}
