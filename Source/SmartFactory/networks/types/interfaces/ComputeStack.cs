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

using SuperComicLib.Runtime;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SmartFactory
{
    [StructLayout(LayoutKind.Sequential, Pack = sizeof(long))]
    public readonly unsafe struct ComputeStack<T>
        where T : unmanaged
    {
        internal readonly T[] _stack;
        internal readonly int _count;

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
            ? _stack!.refdata(_count - 1)
            : default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T PopFast()
        {
            ref int count = ref XUnsafe.AsRef(in _count);
            return _stack!.refdata(--count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Push(T value)
        {
            ref int count = ref XUnsafe.AsRef(in _count);
            _stack!.refdata(count++) = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Reset() => XUnsafe.AsRef(in _count) = 0;
    }
}
