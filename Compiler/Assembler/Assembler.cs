using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Assembler
{
    static class Assembler
    {
        public static void Invoke(string filename)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "ILAsm.Win.x64\\ilasm.exe";
            startInfo.Arguments = "temp.il /DLL /QUIET";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            Console.WriteLine("Assembler returns {0}.", process.ExitCode);
        }
    }
}
