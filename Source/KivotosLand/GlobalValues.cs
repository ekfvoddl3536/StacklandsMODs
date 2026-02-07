// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace KivotosLand;

internal static class GlobalValues
{
    public static Shader shader_spriteDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Load()
    {
        shader_spriteDefault = Shader.Find("Sprites/Default");

        if (shader_spriteDefault == (object)null)
            throw new InvalidOperationException("Can't find Shader!");
    }
}
