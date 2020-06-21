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
            startInfo.Arguments = filename + " /DLL /QUIET";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                Console.WriteLine("ilasm returns {0}.", process.ExitCode);
            }
        }
    }
}
