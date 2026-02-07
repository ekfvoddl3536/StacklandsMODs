// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands;

namespace BiggerCapacity;

internal static class ModOptions
{
    public static int resMax;
    public static int coinMax;

    public static void Load(ConfigFile config)
    {
        resMax =
            config
                .GetEntry(nameof(resMax), 3_000, false)
                .SetOnChanged(new Action<int>(ApplyHelper.ResourceMax))
                .Value;

        coinMax =
            config
                .GetEntry(nameof(coinMax), 10_000, false)
                .SetOnChanged(new Action<int>(ApplyHelper.CoinMax))
                .Value;
    }
}
