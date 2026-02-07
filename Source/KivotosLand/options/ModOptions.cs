// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands;

namespace KivotosLand;

internal static class ModOptions
{
    public static bool onDbg;
    public static bool onFCR;
    public static bool noGacha;

    public static void Load(ConfigFile config)
    {
        onDbg = config.GetValue(nameof(onDbg), false, true);
        onFCR = config.GetValue(nameof(onFCR), true, true);
        noGacha = config.GetValue(nameof(noGacha), false, true);
    }
}
