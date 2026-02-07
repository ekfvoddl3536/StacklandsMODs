// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

[HarmonyPatch(typeof(CreatePackLine), nameof(CreatePackLine.CreateBoosterBoxes))]
public static class CreatePackLine_CreateBoosterBoxes_PATCH01
{
    public static void Prefix(List<string> boosters) => 
        boosters.Add("ba_gacha1_booster");
}
