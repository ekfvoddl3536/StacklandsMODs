// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

/// <summary>
/// <see cref="OperatorType"/> name table
/// </summary>
internal static class OPNameTable
{
    public static readonly string[] Names =
    {
        // def
        "2 ADD ( + )",
        "2 SUB ( - )",
        "2 MUL ( * )",
        "2 DIV ( / )",
        "2 MOD ( % )",

        // bit
        "2 AND ( & )",
        "2 OR ( | )",
        "2 XOR ( ^ )",

        // compare
        "2 EQ ( == )",
        "2 NE ( != )",
        "2 LE ( <= )",
        "2 GE ( >= )",
        "2 LT ( < )",
        "2 GT ( > )",

        // unary
        "1 NOT",
        "1 NEG",
        "1 ABS"
    };
}
