using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            IILEntity entity;
            ILImmediateNumber imm = new ILImmediateNumber();
            ILVariable var = new ILVariable();
            entity = imm;
            Console.WriteLine(entity is ILImmediateNumber);
            Console.WriteLine(entity is ILVariable);
            var m = Regex.Match("asdf", @"asd(?<name>t)?f");
            Console.WriteLine(m.Groups["name"].Success);
        }
    }
}
