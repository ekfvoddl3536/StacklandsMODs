// MIT License
//
// Copyright (c) 2024. SuperComic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#pragma warning disable IDE1006
#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SuperComicLib.Runtime
{
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
}
