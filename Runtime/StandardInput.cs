using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class StandardInput : IInputStream
    {
        private int buf;
        private bool readed;
        public StandardInput()
        {
            buf = 1;
            readed = true;
        }
        public int ReadChar()
        {
            if(readed)
                buf = Console.Read();
            readed = true;
            return buf;
        }
        public void UnReadChar()
        {
            if (!readed)
                throw new Exception("Stdin Error: consecutive unread");
            readed = false;
        }
    }
}
