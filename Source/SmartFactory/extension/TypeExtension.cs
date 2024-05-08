﻿// MIT License
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

using fluxiolib;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SmartFactory
{
    internal static class TypeExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeFieldAccessor GetInstanceFieldAccessor(this Type type, string name) =>
            FluxTool.GetFieldAccessor(GetInstanceFieldInfo(type, name));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldInfo GetInstanceFieldInfo(this Type type, string name)
        {
            const BindingFlags INSTANCE_ALL = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            return type.GetField(name, INSTANCE_ALL);
        }
    }
}