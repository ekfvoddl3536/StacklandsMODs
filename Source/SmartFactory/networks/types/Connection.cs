// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

[StructLayout(LayoutKind.Sequential, Pack = sizeof(long))]
public readonly struct Connection<T>
    where T : class, ILogicGate
{
    public readonly T reference;
    public readonly int index;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Connection(T reference, int index)
    {
        this.reference = reference;
        this.index = index;
    }
}
