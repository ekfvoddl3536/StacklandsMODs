// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;
using System.Reflection;

namespace KivotosLand;

[HarmonyPatch(typeof(CardopediaScreen), "Update")]
internal static class CardopediaScreen_Update_PATCH01
{
    private static FieldInfo _entries, _SearchDisabled, _totalFoundCount, _currentTotalCardCount;

    private static UpdateTabsDelegate _UpdateTabs;

    public static void Prepare()
    {
        const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

        var t = typeof(CardopediaScreen);

        _entries = t.GetField("entries", FLAGS);
        _SearchDisabled = t.GetField("SearchDisabled", FLAGS);
        _totalFoundCount = t.GetField("totalFoundCount", FLAGS);
        _currentTotalCardCount = t.GetField("currentTotalCardCount", FLAGS);

        _UpdateTabs = (UpdateTabsDelegate)t.GetMethod("UpdateTabs", FLAGS).CreateDelegate(typeof(UpdateTabsDelegate));

        if (_entries == null ||
            _SearchDisabled == null ||
            _totalFoundCount == null ||
            _currentTotalCardCount == null ||
            _UpdateTabs == null)
            throw new InvalidOperationException("no Field");
    }

    public static bool Prefix(CardopediaScreen __instance)
    {
        __instance.HoveredEntry = null;

        if (GameCanvas.instance.ScreenIsInteractable<CardopediaScreen>())
            SelectHoverEntry(__instance);

        var lastHover = CardopediaHelper._lastHoveredEntry;
        if (lastHover != null)
            lastHover.Button.Image.color = ColorManager.instance.ButtonColor;

        var hovEntry = __instance.HoveredEntry;
        if (hovEntry != null)
            hovEntry.Button.image.color = ColorManager.instance.HoverButtonColor;

        __instance.UpdatePositions();

        GameCard demoCard = CardopediaHelper._demoCard;
        if (lastHover != hovEntry && hovEntry != null)
        {
            demoCard = CardopediaHelper.RefreshDemoCard(hovEntry.MyCardData, hovEntry.wasFound);
            CardopediaHelper._demoCard = demoCard;
        }

        if (demoCard != null)
            demoCard.transform.position = demoCard.TargetPosition = __instance.TargetCardPos.position;

        if (hovEntry != null)
        {
            var cardDesc = __instance.CardDescription;
            cardDesc.transform.parent.gameObject.SetActive(true);
            if (hovEntry.wasFound)
            {
                demoCard.CardData.UpdateCardText();

                var dropSummary = CardopediaHelper.GetDropSummaryFromCard(hovEntry.MyCardData);
                string str;
                if (hovEntry.MyCardData is Blueprint blueprint)
                    str = blueprint.GetText();
                else
                {
                    str = demoCard.CardData.Description.Replace("\\d", "\n\n");

                    if (dropSummary != null && hovEntry.MyCardData.MyCardType != CardType.Locations)
                        str += "\n\n" + dropSummary;
                }

                cardDesc.text = str;
            }
            else
                cardDesc.text = SokLoc.Translate(SokTerms.label_card_not_found);
        }

        __instance.SearchField.gameObject.SetActive(!InputController.instance.CurrentSchemeIsController && !(bool)_SearchDisabled.GetValue(__instance));

        var totalCnt = (int)_totalFoundCount.GetValue(__instance);
        var currCnt = (int)_currentTotalCardCount.GetValue(__instance);

        __instance.CardFoundAmount.text =
            SokLoc.Translate(
            SokTerms.label_cards_found,
            LocParam.Create("found", totalCnt.ToString()),
            LocParam.Create("total", currCnt.ToString()));

        CardopediaHelper._lastHoveredEntry = hovEntry;

        _UpdateTabs.Invoke(__instance);

        return false;
    }

    private static void SelectHoverEntry(CardopediaScreen @this)
    {
        var list = (List<CardopediaEntryElement>)_entries.GetValue(@this);

        foreach (var e in list)
        {
            var btn = e.Button;
            if (btn.IsHovered || btn.IsSelected)
            {
                @this.HoveredEntry = e;
                break;
            }
        }
    }
}
