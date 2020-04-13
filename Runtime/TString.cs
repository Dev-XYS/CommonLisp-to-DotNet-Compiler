using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class TString : IDataType
    {
        public string Value { get; set; }
        public override string ToString()
        {
            return "\"" + Value.ToString() + "\"";
        }

        public TString(string s)
        {
            Value = s;
        }

        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
