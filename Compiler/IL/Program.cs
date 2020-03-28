using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    /// <summary>
    /// 表示一个完整的中间代码程序。
    /// 一个程序由一下两部分构成：
    /// (1) 全体函数集合；
    /// (2) 主程序代码。
    /// 对每个函数分别编译为一个Function对象，储存在FunctionLisp中。
    /// 将主程序编译后储存在Main中。
    /// </summary>
    class Program
    {
        /// <summary>
        /// 全体函数列表
        /// </summary>
        public List<Function> FunctionList { get; }

        /// <summary>
        /// 主程序
        /// </summary>
        public List<IInstruction> Main { get; }

        public Program()
        {
            FunctionList = new List<Function>();
            Main = new List<IInstruction>();
        }
    }
}
