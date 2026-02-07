// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand;

[HarmonyPatch(typeof(CardopediaScreen), "ClearScreen")]
internal static class CardopediaScreen_ClearScreen_PATCH01
{
    public static bool Prefix(CardopediaScreen __instance)
    {
        var demoCard = CardopediaHelper._demoCard;
        if (demoCard != null)
        {
            UnityEngine.Object.Destroy(demoCard.gameObject);
            CardopediaHelper._demoCard = null;
        }

        __instance.CardDescription.transform.parent.gameObject.SetActive(false);
        CardopediaHelper._lastHoveredEntry = null;

        var background = __instance.CardopediaBackground;
        if (background != null)
            background.gameObject.SetActive(false);

        return false;
    }
}
