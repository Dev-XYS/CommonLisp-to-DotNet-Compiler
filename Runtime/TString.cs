using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class TString : IType
    {
        public string Value { get; set; }
        public override string ToString()
        {
            return Value.ToString();
        }
        public TString()
        {
            Value = "";
        }
        public TString(string s)
        {
            Value = s;
        }
    }
}
