// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands;
using UnityEngine;

namespace KivotosLand;

internal static class ModOptions
{
    public static bool onDbg;
    public static bool onFCR;
    public static bool noGacha;

    public static float pickupVolume;

    private static ConfigEntry<int> s_entry_pickupVolume;

    public static void Load(ConfigFile config)
    {
        onDbg = config.GetValue(nameof(onDbg), false, true);
        onFCR = config.GetValue(nameof(onFCR), true, true);
        noGacha = config.GetValue(nameof(noGacha), false, true);

        var entry_volume = config.GetEntry(nameof(pickupVolume), 35, false);
        s_entry_pickupVolume = entry_volume;
        entry_volume.OnChanged += OnVolumeUpdated;
        OnVolumeUpdated(entry_volume.Value);
    }

    private static void OnVolumeUpdated(int value)
    {
        var clamped_value = Mathf.Clamp(value, 0, 100);
        if (clamped_value != value)
        {
            // 이 호출을 하면 이 메소드가 다시 호출됩니다
            s_entry_pickupVolume?.Value = clamped_value;
        }
        else
        {
            pickupVolume = 0.5f * (clamped_value / 100f);
        }
    }
}
