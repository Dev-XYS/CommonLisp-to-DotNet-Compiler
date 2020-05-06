using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Runtime
{
    public class FileInput : IInputStream
    {
        private StreamReader reader;
        private int buf = 1;
        private bool readed = true;
        public FileInput(string path)
        {
            reader = new StreamReader(path);
            
        }
        public int ReadChar()
        {
            if (readed)
                buf = reader.Read();
            readed = true;
            return buf;
        }

        public void UnReadChar()
        {
            if (!readed)
                throw new RuntimeException("filein error: consecutive unread");
            readed = false;
        }
    }
}
