using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class Variable : IEntity
    {
        public string Name { get; }

        public Environment Env { get; set; }

        public Variable(Environment env)
        {
            Env = env;
        }
    }
}
