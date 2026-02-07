// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using System.Reflection;
using SuperComicLib.Stacklands;

namespace KivotosLand;

public sealed class ModLoad : Mod
{
    private const int MAX_PHASE = 4;
    private const int MAX_COIN_COUNT = 1200;

    public override void Ready()
    {
        var logger = Logger;
        logger.Log(nameof(KivotosLand) + " MOD Loading... by 'SuperComic (ekfvoddl3535@naver.com)'");

        try
        {
            LogPhase(logger,1);

            this.LoadFallbackTerms();

            ModOptions.Load(Config);
            GlobalValues.Load();

            LogPhase(logger,2);

            var t = typeof(Combatable);

            const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;
            Student._AttackSpecialHit = t.GetField("AttackSpecialHit", FLAGS) ?? throw new FieldAccessException();
            Student._PerformSpecialHit = (PerformSpecialHitDelegate)t.GetMethod("PerformSpecialHit", FLAGS).CreateDelegate(typeof(PerformSpecialHitDelegate));
            Student._ShowHitText = (ShowHitTextDelegate)t.GetMethod("ShowHitText", FLAGS).CreateDelegate(typeof(ShowHitTextDelegate));

            LogPhase(logger,3);

            var temp_selector = new Func<CardData, bool>(CHEST_SELECTOR);
            var chestPrefabs = WorldManager.instance.CardDataPrefabs.AsParallel().AsUnordered().Where(temp_selector);

            foreach (var prefab in chestPrefabs)
            {
                ref int maxCoins = ref ((Chest)prefab).MaxCoinCount;

                if (maxCoins < MAX_COIN_COUNT)
                    maxCoins = MAX_COIN_COUNT;
            }

            LogPhase(logger, 4);

            PostModLoad();

            this.PatchAllWithDependencies(Harmony, false);
            Harmony.PatchAll();

            logger.Log(nameof(KivotosLand) + " MOD Loaded!");
        }
        catch (Exception e)
        {
            logger.Log(nameof(KivotosLand) + " MOD Load FAIL! -> " + e.ToString());
        }
    }

    private static void PostModLoad()
    {
        if (!ModOptions.noGacha)
            return;

        var _where = new Func<CardChance, bool>(CARDCHANCE_FILTER);
        var _select = new Func<CardChance, CardChance>(CARDCHANCE_SELECTOR);

        var datas = WorldManager.instance.GameDataLoader.BoosterpackDatas;
        foreach (var data in datas)
            if (data.BoosterId == "ba_gacha1_booster")
            {
                foreach (var cardBag in data.CardBags)
                {
                    var list = cardBag.Chances.Where(_where).Select(_select).ToList();
                    list.Add(new CardChance(Cards.gold, 1));

                    cardBag.Chances.Clear();
                    cardBag.Chances = list;
                }

                break;
            }

        GC.Collect(0, GCCollectionMode.Default, false, false);
    }

    private static void LogPhase(ModLogger logger, int phase) =>
        logger.Log(nameof(KivotosLand) + $" MOD Initialize... Phase {phase} / {MAX_PHASE}");

    private static bool CHEST_SELECTOR(CardData x) => x is Chest;

    private static bool CARDCHANCE_FILTER(CardChance x) => x.Id.StartsWith("ba_");

    private static CardChance CARDCHANCE_SELECTOR(CardChance x)
    {
        x.Chance = (int)Math.Min((long)x.Chance * 100_000, int.MaxValue);
        return x;
    }
}
