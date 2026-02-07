// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib;
using SuperComicLib.Runtime;
using UnityEngine;

namespace SmartFactory;

static partial class NetworkGrids
{
    public static void UpdateCache()
    {
        var list = WorldManager.instance.AllCards;

        cachedCards =
            list.Count < 32
            ?
                list
                .Where(x => x.CardData is LogicBase lb && lb.Inputs.Length == 0 && lb.MyGameCard.Parent == false)
                .Select(x => (LogicBase)x.CardData)
                .ToArray()
            :
                // 순서 신경 안씀
                list
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .AsUnordered()
                .Where(x => x.CardData is LogicBase lb && lb.Inputs.Length == 0 && lb.MyGameCard.Parent == false)
                .Select(x => (LogicBase)x.CardData)
                .ToArray();

        if (L1Nodes._items.Length < cachedCards.Length)
        {
            int next = L1Nodes._items.Length << 1;

            int pow2 = cachedCards.Length;
            if ((pow2 & (pow2 - 1)) != 0)
                pow2 = (int)BitMath.SetUnderbits((uint)pow2) + 1;

            // 더 큰 크기로 할당하려고 시도하고, 실패하면
            // 조금 더 작은 크기로 다시 시도
            // 그것조차 실패하면 오류를 내보낸다
            int size = Mathf.Max(pow2, next);
            for (int retry = 0; ; ++retry)
                try
                {
                    // fast allocation
                    XUnsafe.AsRef(in L1Nodes._items) = new LogicBase[size];
                    XUnsafe.AsRef(in L2Nodes._items) = new LogicBase[size];

                    currentHash.ForceSetCapacity(size);
                    break;
                }
                catch (OutOfMemoryException OutOfMem)
                {
                    // is insuffiction memory?
                    if (retry > 0)
                        throw OutOfMem;

                    size = Mathf.Min(pow2, next);
                }
        }
    }
}
