// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace SuperComicLib.Stacklands.Collections;

public struct FastStack<T>
{
    public readonly T[] _items;
    public int Count;

    public FastStack(int length)
    {
        _items = new T[Mathf.Max(length, 4)];
        Count = 0;
    }

    public readonly ref T FirstElementByRef =>
        // @DISABLE_NO_CHECK
        // ref _items!.refdata();
        ref _items[0];

    public void Push(T value) =>
        // @DISABLE_NO_CHECK
        // _items!.refdata(Count++) = value;
        _items[Count++] = value;

    public void Clear()
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            _items.AsSpan(0, Count).Clear();

        Count = 0;
    }
}
