using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    /// <summary>
    /// 可用来表示一个范围内的全部或部分变量。
    /// </summary>
    class Environment
    {
        public string Name { get; set; }

        /// <summary>
        /// 变量列表
        /// </summary>
        public List<Variable> VariableList { get; }

        public Environment(string name)
        {
            Name = name;
            VariableList = new List<Variable>();
        }
    }
}
