// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands.Collections;
using Unity.Jobs;

namespace SmartFactory;

internal static partial class NetworkGrids
{
    public static void Initialize()
    {
        const int DEFAULT_SIZE = 8;

        cachedCards = Array.Empty<LogicBase>();

        L1Nodes = new FastStack<LogicBase>(DEFAULT_SIZE);
        L2Nodes = new FastStack<LogicBase>(DEFAULT_SIZE);

        currentHash = new FastHashSet<LogicBase>(DEFAULT_SIZE);
    }

    public static void Update()
    {
        if (clearJobHandle.IsCompleted == false)
            clearJobHandle.Complete();

        _UpdateCore();

        clearJobHandle = new HashClearJob().Schedule();
    }
}
