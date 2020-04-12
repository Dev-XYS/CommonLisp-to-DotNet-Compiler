﻿using System;

using Runtime;

static class Program
{
    static void Main(string[] args)
    {
        IType temp = Constants.const1;
    }
}

static class Constants
{
    public static readonly IType const1 = new TInteger(8);
}

class E0
{
    public IType minus;
}

class E1
{
    public IType balance;
    public IType temp;
}

class E2
{
    public IType x;
}

class anonymous : IType
{
    private E0 @E0;
    private E1 @E1;
    private E2 @E2;

    public anonymous(E0 e0, E1 e1)
    {
        @E0 = e0;
        @E1 = e1;
    }

    public IType Invoke(IType[] args)
    {
        @E2.x = null;

        @E2.x = args[0];
        IType[] _args = new IType[2];
        _args[0] = @E1.balance;
        _args[1] = @E2.x;

        return @E0.minus.Invoke(_args);
    }
}

class create_account : IType
{
    private E0 @E0;
    private E1 @E1;

    public create_account(E0 e0)
    {
        @E0 = e0;
    }

    public IType Invoke(IType[] args)
    {
        @E1.balance = null;
        @E1.temp = null;

        @E1.temp = new anonymous(@E0, @E1);

        return @E1.temp;
    }
}

class main0 : IType
{
    private E0 @E0;

    public main0()
    {
    }

    public IType Invoke(IType[] args)
    {
        IType t = new TInteger(1);
        return null;
    }
}
