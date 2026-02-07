// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
#nullable enable
namespace SuperComicLib.Runtime;

public unsafe delegate void ParallelQueryAction<T>(Span<T> batch);

internal static class ArrayExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T refdata<T>(this T[] values) => ref Unsafe.As<byte, T>(ref XUnsafe.As<RawArrayObject>(values!)!.Data);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T refdata<T>(this T[] values, int index) => ref XUnsafe.Add(ref refdata(values!), index);

#pragma warning disable
    private sealed class RawArrayObject
    {
        public readonly IntPtr Bounds;
        public readonly IntPtr Length;

        public byte Data;
    }
#pragma warning restore
}

internal static class CollectionExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T[] GetBufferReference<T>(this List<T> list) => 
        ref Unsafe.As<RawListObject<T>>(list)._items;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> GetBufferReferenceAsSpan<T>(this List<T> list) =>
        MemoryMarshal.CreateSpan(ref GetBufferReference(list).refdata(), list.Count);

#pragma warning disable
    private sealed class RawListObject<T>
    {
        public T[] _items;

        // NOTE: 이 순서는 부정확할 수 있음
        public int _count;
        public int _version;
    }
#pragma warning restore
}
