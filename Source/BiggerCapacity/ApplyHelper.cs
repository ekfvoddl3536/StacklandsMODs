// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace BiggerCapacity;

internal static class ApplyHelper
{
    private static Func<CardData, bool> _resourceChestFilter;
    private static Func<CardData, bool> _coinChestFilter;

    public static void Initialize()
    {
        _resourceChestFilter = new Func<CardData, bool>(RESOURCE_CHEST_SELECTOR);
        _coinChestFilter = new Func<CardData, bool>(COIN_CHEST_SELECTOR);
    }

    public static void ResourceMax(int changedValue)
    {
        foreach (var c in Filtered(_resourceChestFilter))
            ((ResourceChest)c).MaxResourceCount = changedValue;
    }

    public static void CoinMax(int changedValue)
    {
        foreach (var c in Filtered(_coinChestFilter))
            ((Chest)c).MaxCoinCount = changedValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IEnumerable<CardData> Filtered(Func<CardData, bool> filter) =>
        WorldManager.instance.CardDataPrefabs.Where(filter);

    private static bool COIN_CHEST_SELECTOR(CardData x) => x is Chest;
    private static bool RESOURCE_CHEST_SELECTOR(CardData x) => x is ResourceChest;
}
