// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

internal static class ModPostLoad
{
    public static void Ready()
    {
        var loader = WorldManager.instance.GameDataLoader;

        // 카드 출현 확률을 설정하기 위해 카드를 등록
        RegisterCards(loader);

        // 카드를 출현시키기 위해 부스터팩에 목록으로 추가
        ApplyBoosterpacks(loader);
    }

    private static void ApplyBoosterpacks(GameDataLoader loader)
    {
        var list = loader.BoosterpackDatas;

        foreach (var w in list.Where(x => x.BoosterLocation == Location.Mainland))
            if (w.BoosterId.Equals("structures", StringComparison.OrdinalIgnoreCase))
            {
                AddCardBag(w.CardBags, SetCardBagType.AdvancedBuildingIdea);

                break;
            }
    }

    [SkipLocalsInit]
    private static unsafe void RegisterCards(GameDataLoader loader)
    {
        const ulong K_MAP =
            1ul << (int)SetCardBagType.AdvancedBuildingIdea |
            1ul << (int)SetCardBagType.Island_AdvancedBuildingIdea |
            1ul << (int)SetCardBagType.Happiness_AdvancedBuildingIdea;

        var bags = loader.SetCardBags;

        // 이 모드에서 추가한 모든 블루프린트 프리펩 조회,
        // SimpleCardChance로 변환
        var myModIdeas = 
            loader.CardDataPrefabs
            .Where(x => x.Id.StartsWith("sf_blueprint_"))
            .Select(x => new SimpleCardChance(x.Id, 1))
            .ToArray();

        var found = 0ul;
        foreach (var data in bags)
            if (((int)(K_MAP >> (int)data.SetCardBagType) & 1) != 0)
            {
                found |= 1ul << (int)data.SetCardBagType;

                data.Chances.AddRange(myModIdeas);
            }

        // 만약 찾지 못한 테마가 있으면, 강제로 추가한다
        var pList = stackalloc SetCardBagType[3];
        pList[0] = SetCardBagType.AdvancedBuildingIdea;
        pList[1] = SetCardBagType.Island_AdvancedBuildingIdea;
        pList[2] = SetCardBagType.Happiness_AdvancedBuildingIdea;

        for (int i = 0; i < 3; ++i)
            if (((int)(found >> (int)pList[i]) & 1) == 0)
                bags.Add(new SetCardBagData
                {
                    SetCardBagType = pList[i],
                    Chances = new List<SimpleCardChance>(myModIdeas)
                });
    }

    private static void AddCardBag(List<CardBag> myBags, SetCardBagType type)
    {
        if (!myBags.Any(x => x.CardBagType == CardBagType.SetCardBag && x.SetCardBag == type))
            myBags.Add(new CardBag
            {
                CardBagType = CardBagType.SetCardBag,
                CardsInPack = 1,
                SetCardBag = type,
                UseFallbackBag = false,
            });
    }
}