// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands.Collections;
using Unity.Jobs;

namespace SmartFactory;

static partial class NetworkGrids
{
    internal static LogicBase[] cachedCards;

    internal static FastStack<LogicBase> L1Nodes;
    internal static FastStack<LogicBase> L2Nodes;

    internal static FastHashSet<LogicBase> currentHash;

    private static JobHandle clearJobHandle;

    public readonly struct HashClearJob : IJob
    {
        void IJob.Execute() => currentHash.Clear();
    }
}
