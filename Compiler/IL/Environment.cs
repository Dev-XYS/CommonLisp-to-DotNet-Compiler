using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    /// <summary>
    /// 可用来表示一个范围内的全部或部分变量。
    /// </summary>
    public class Environment
    {
        private static int geid;
        private int eid;

        public string Name { get; }

        /// <summary>
        /// 变量列表
        /// </summary>
        public List<Variable> VariableList { get; }

        public Environment()
        {
            eid = geid++;
            Name = "env" + eid;

            VariableList = new List<Variable>();
        }

        public override int GetHashCode()
        {
            return eid;
        }

        public bool ContainsVariable(Variable var)
        {
            return VariableList.Contains(var);
        }
    }
}
