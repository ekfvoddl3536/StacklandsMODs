// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand;

[HarmonyPatch(typeof(CardopediaScreen), "SetTempDemoCard")]
internal static class CardopediaScreen_SetTempDemoCard_PATCH01
{
    public static bool Prefix(CardopediaScreen __instance, CardData data)
    {
        var demoCard = CardopediaHelper.RefreshDemoCard(data);
        var cardData = demoCard.CardData;

        var cardDesc = __instance.CardDescription;
        cardDesc.transform.gameObject.SetActive(true);

        cardData.UpdateCardText();

        var dropSummary = CardopediaHelper.GetDropSummaryFromCard(cardData);
        string str;

        if (cardData is Blueprint blueprint)
            str = blueprint.GetText();
        else
        {
            str = cardData.Description.Replace("\\d", "\n\n");

            if (cardData is Combatable combatable)
                str += combatable.GetCombatableDescriptionAdvanced();

            if (dropSummary != null && cardData.MyCardType != CardType.Locations)
                str += "\n\n" + dropSummary;
        }

        cardDesc.text = str;

        CardopediaHelper._demoCard = demoCard;

        return false;
    }
}
