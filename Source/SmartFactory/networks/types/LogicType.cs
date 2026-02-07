// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

[Flags]
public enum LogicType
{
    TypeMask = 0x000F_FFFF,

    Any = 0,

    Boolean,

    Number,

    TypeNB,

    // TagMask = 0x3FF0_0000,
    // 
    // Tag1 = 1 << 20,
    // Tag2 = Tag1 * 2,
    // Tag3 = Tag1 * 3,
    // Tag4 = Tag1 * 4,
    // 
    // TagNB = Tag1 * 5,

    Connected = int.MinValue
}
