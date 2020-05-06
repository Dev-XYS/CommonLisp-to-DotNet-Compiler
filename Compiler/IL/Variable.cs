using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class Variable : IEntity
    {
        private static int gvid;
        private int vid;

        public string Name { get; set; }

        public Environment Env { get; set; }

        public Variable(Environment env)
        {
            vid = gvid++;
            Name = "temp" + this.GetHashCode().ToString();
            Env = env;
        }

        public Variable(string name, Environment env)
        {
            Name = name;
            Env = env;
        }

        public override string ToString()
        {
            return Env.Name + "::" + Name;
        }

        public override int GetHashCode()
        {
            return vid;
        }
    }

    class UnresolvedObject : IEntity
    {
        public string Name { get; }

        public UnresolvedObject(string name)
        {
            Name = name;
        }
    }
}
