using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Optimization
{
    class Program
    {
        public IEnumerable<Function> FunctionList { get; set; }

        public IEnumerable<IL.Environment> EnvList { get; set; }

        public Function Main { get; set; }

        public Program(IL.Program program)
        {
            // Copy properties from the base class.
            FunctionList = program.FunctionList.ToList().ConvertAll((IL.Function func) => new Function(func));
            EnvList = program.EnvList;
            Main = new Function(program.Main);
        }
    }
}
