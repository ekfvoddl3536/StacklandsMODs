// MIT License
//
// Copyright (c) 2022 Benedikt Werner
// Copyright (c) 2024 SuperComic (ekfvoddl3535@naver.com)
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

using System.Collections;
using UnityEngine;

namespace FasterEndOfMonths;

internal readonly struct NoWaitCoroutine : IEnumerator
{
    private readonly IEnumerator _original;

    public NoWaitCoroutine(IEnumerator original) => _original = original;

    public bool MoveNext() => _original.MoveNext();
    public void Reset() => _original.Reset();
    public object Current => FilterCurrent(_original.Current);

    private static object FilterCurrent(object v)
    {
        // any wait source -> no wait
        if (v is WaitForSeconds ||
            v is WaitForFixedUpdate ||
            v is WaitForEndOfFrame ||
            v is WaitForSecondsRealtime)
            return null;

        return v;
    }
}
