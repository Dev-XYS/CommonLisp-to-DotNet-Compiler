# Common Lisp 到 .NET 平台的编译器

## 开发环境

Windows 平台下的 Visual Studio，需要安装 .NET Core 开发工具 3.1 版本。

## 编译项目

本项目没有除了 .NET 基础类库没有其他依赖，将解决方案载入 Visual Studio 直接编译即可。汇编器不是本项目的一部分，它直接以可执行文件提供，需要将压缩包 `ILAsm.Win.x64.zip` 解压到文件夹 `/Compiler/bin/Debug/netcoreapp3.1` 下（注意：不要为压缩包中的内容创建子文件夹）。

## 编译器的使用

在项目编译完成后，文件夹 `/Compiler/bin/Debug/netcoreapp3.1` 下 `Compiler.exe` 即为编译器的可执行文件。直接运行此程序（无参数）将从标准输入读取 Lisp 代码，编译为 `Program.dll`。运行 `Program.exe` 会调用 `Program.dll`，执行编译出的程序。

`Compiler.exe` 提供几个参数：

- `-l` ：编译为库，此时编译出的文件为 `Library.dll`。
- `-i` ：运行解释器。
- `-ii` ：运行 REPL。
- `[FILE]` ：将 `FILE` 作为输入编译。

## 代码目录结构

- 项目 `Compiler` ：编译器
    - `Assembler` ：汇编器调用
    - `CIL` ：CIL指令及代码生成
        - `Instructions` ：CIL汇编中使用到的指令定义
    - `Frontend` ：前端
    - `IL` ：中间语言定义
    - `Optimization` ：优化
        - `ControlFlow` ：数据流分析
    - `Resources` ：CIL导言
- 项目 `Runtime` ：运行时
- `CompilerTest` ：测试用例及脚本
