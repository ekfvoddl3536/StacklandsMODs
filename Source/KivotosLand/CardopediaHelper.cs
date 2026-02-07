// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace KivotosLand;

internal static class CardopediaHelper
{
    public static GameCard _demoCard;
    public static CardopediaEntryElement _lastHoveredEntry;

    public static GameCard RefreshDemoCard(CardData data, bool faceUp = true)
    {
        var demoCard = _demoCard;
        if (demoCard != null)
            UnityEngine.Object.Destroy(demoCard.gameObject);

        demoCard = UnityEngine.Object.Instantiate(PrefabManager.instance.GameCardPrefab);

        var cardData = UnityEngine.Object.Instantiate(data);
        cardData.transform.SetParent(demoCard.transform);

        demoCard.CardData = cardData;
        cardData.MyGameCard = demoCard;

        demoCard.FaceUp = faceUp;
        demoCard.IsDemoCard = true;

        demoCard.SetDemoCardRotation();
        demoCard.UpdateCardPalette();

        if (cardData is Student student)
            student.Initialize();

        cardData.UpdateCard();

        demoCard.ForceUpdate();

        return demoCard;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDropSummaryFromCard(CardData cardData) =>
        cardData is Harvestable || cardData is Enemy
        ? BoosterpackData.GetSummaryFromAllCards(cardData.GetPossibleDrops(), SokTerms.label_can_drop)
        : string.Empty;
}
