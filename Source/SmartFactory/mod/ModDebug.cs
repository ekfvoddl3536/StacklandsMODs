// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace SmartFactory;

internal static class ModDebug
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Log(string message)
    {
        if (ModOptions.onDebug)
            Debug.Log($"[{nameof(SmartFactory)} DEBUG] {message}");
    }
}
