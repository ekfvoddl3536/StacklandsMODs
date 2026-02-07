// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
namespace SmartFactory;

partial class LogicBase
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static void _ResetConnect(LogicType[] types)
    {
        for (int i = 0; i < types.Length; ++i)
            types[i] &= ~LogicType.Connected;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static bool _CanConnect(LogicType[] vs)
    {
        var flags = LogicType.Connected;

        for (int i = 0; i < vs.Length; ++i)
            flags &= vs[i];

        return flags >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static bool _HasNode(LogicBase[] vs, LogicBase card)
    {
        for (int i = 0; i < vs.Length; ++i)
            if (vs[i] == (object)card)
                return true;

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static int _GetConnections(LogicType[] items)
    {
        int count = 0;
        for (int i = 0; i < items.Length; ++i)
            count += (int)((uint)items[i] >> 31);

        return count;
    }
}
