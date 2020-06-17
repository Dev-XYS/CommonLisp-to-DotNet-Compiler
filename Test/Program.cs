using System;
using System.Reflection;
using Runtime;

static class Program
{
    static void Main(string[] args)
    {
        Assembly testa = Assembly.LoadFrom("Program.dll");
        Type[] type = testa.GetTypes();
        foreach(var tp in type)
        {
            Console.WriteLine(tp.FullName);
        }
        LibMain main = new LibMain();
        
    }
}
