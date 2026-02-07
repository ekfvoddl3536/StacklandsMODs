// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace SmartFactory;

[HarmonyPatch(typeof(HeavyFoundation), "CanHaveCard")]
internal static class HeavyFoundation_CanHaveCard_PATCH01
{
    public static bool Prefix(ref bool __result, CardData otherCard)
    {
        // LogicBase 이 외 모든 카드는 HeavyFoundation이 소유 가능하다
        __result = !(otherCard is LogicBase);

        return false;
    }
}
