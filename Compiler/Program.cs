using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = Regex.Match("asdf", @"asd(?<name>t)?f");
            Console.WriteLine(m.Groups["name"].Success);
        }
    }
}
