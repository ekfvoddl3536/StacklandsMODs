// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace SmartFactory;

[HarmonyPatch(typeof(Smelter), "CanHaveCard")]
internal static class Smelter_CanHaveCard_PATCH01
{
    public static bool Prefix(ref bool __result, CardData otherCard)
    {
        var id = otherCard.Id;

        __result =
            id.Length switch
            {
                4 => 
                    id == Cards.wood ||
                    id == Cards.sand ||
                    id == Cards.gold,

                5 => 
                    id == Cards.glass,

                8 =>
                    id == Cards.iron_ore ||
                    id == Cards.gold_ore ||
                    id == Cards.gold_bar ||
                    id == Cards.charcoal,

                _ => false,
            };

        return false;
    }
}
