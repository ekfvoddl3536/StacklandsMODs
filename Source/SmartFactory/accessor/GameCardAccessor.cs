// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using fluxiolib;

namespace SmartFactory;

internal static class GameCardAccessor
{
    public static readonly UnsafeFieldAccessor propBlock;

    static GameCardAccessor()
    {
        var t = typeof(GameCard);

        propBlock = t.GetInstanceFieldAccessor(nameof(propBlock));
    }
}
