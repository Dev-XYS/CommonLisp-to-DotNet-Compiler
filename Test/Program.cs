using System;
using System.Reflection;
using Runtime;

static class Program
{
    static void Main(string[] args)
    {
        Assembly testa = Assembly.LoadFrom("Library.dll");
        var tlm = testa.GetType("LibMain");
        var lm = Activator.CreateInstance(tlm);
        var m = tlm.GetMethod("Invoke");
        m.Invoke(lm, new object[] { new IType[0]});
        var tglb = tlm.GetField("global");
        var global = tglb.GetValue(lm);
        var f = testa.GetType("global").GetField("TESTL").GetValue(global);
        m = testa.GetType("TESTL").GetMethod("Invoke");
        IType[] types = new IType[3];
        types[0] = new TInteger(5);
        types[1] = new TInteger(5);
        types[2] = new TInteger(7);
        object[] vs = new object[] { types };
        Console.WriteLine(m.Invoke(f, vs));
    }
}
