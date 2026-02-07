// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

public enum OperatorType
{
    /// <summary>
    /// x + y
    /// </summary>
    Add,

    /// <summary>
    /// x - y
    /// </summary>
    Sub,
    
    /// <summary>
    /// x * y
    /// </summary>
    Mul,

    /// <summary>
    /// x / y
    /// </summary>
    Div,

    /// <summary>
    /// x % y
    /// </summary>
    Mod,

    /// <summary>
    /// x &amp; y
    /// </summary>
    And,

    /// <summary>
    /// x | y
    /// </summary>
    Or,
    
    /// <summary>
    /// x ^ y
    /// </summary>
    Xor,

    CmpEQ,
    CmpNE,
    CmpLE,
    CmpGE,
    CmpLT,
    CmpGT,

    /// <summary>
    /// !x (~x)
    /// </summary>
    Not,

    /// <summary>
    /// -x
    /// </summary>
    Neg,

    /// <summary>
    /// x &lt; 0 ? -x : x
    /// </summary>
    Abs,
}
