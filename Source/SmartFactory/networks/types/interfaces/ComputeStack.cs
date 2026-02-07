// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

// @DISABLE_NO_CHECK
#pragma warning disable

namespace SmartFactory;

[StructLayout(LayoutKind.Sequential, Pack = sizeof(long))]
// @DISABLE_NO_CHECK
// public readonly unsafe struct ComputeStack<T>
public unsafe struct ComputeStack<T>
    where T : unmanaged
{
    // @DISABLE_NO_CHECK
    // internal readonly T[] _stack;
    // internal readonly int _count;
    internal readonly T[] _stack;
    internal int _count;

    public ComputeStack(int maxStackSize)
    {
        _stack = new T[maxStackSize];
        _count = 0;
    }

    public int Count => _count;
    public int MaxStack => _stack!.Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Pop() => 
        (uint)_count >= (uint)_stack!.Length
        ? throw new InvalidOperationException("EmptyCollection")
        : PopFast();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T PeekOrDefault() =>
        _count > 0
        // @DISABLE_NO_CHECK
        // ? _stack!.refdata(_count - 1)
        ? _stack[_count - 1]
        : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal T PopFast()
    {
        // @DISABLE_NO_CHECK
        // ref int count = ref XUnsafe.AsRef(in _count);
        // return _stack!.refdata(--count);
        return _stack[--_count];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Push(T value)
    {
        // @DISABLE_NO_CHECK
        // ref int count = ref XUnsafe.AsRef(in _count);
        // _stack!.refdata(count++) = value;
        _stack[_count++] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Reset() =>
        // @DISABLE_NO_CHECK
        // XUnsafe.AsRef(in _count) = 0;
        _count = 0;
}
