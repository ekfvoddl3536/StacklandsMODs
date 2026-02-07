// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands;

namespace BiggerCapacity;

public sealed class ModLoad : Mod
{
    public override void Ready()
    {
        var logger = Logger;
        logger.Log(nameof(BiggerCapacity) + " MOD Loading... by 'SuperComic (ekfvoddl3535@naver.com)'");

        try
        {
            logger.Log("Load options...");

            this.LoadFallbackTerms();

            ModOptions.Load(Config);

            logger.Log("Done! Apply values...");

            ApplyHelper.Initialize();

            ApplyHelper.ResourceMax(ModOptions.resMax);
            ApplyHelper.CoinMax(ModOptions.coinMax);

            logger.Log("Done! Apply patches...");

            this.PatchAllWithDependencies(Harmony, false);

            logger.Log(nameof(BiggerCapacity) + " MOD Loaded!");
        }
        catch (Exception e)
        {
            logger.Log(nameof(BiggerCapacity) + " MOD Load FAIL! -> " + e.ToString());
        }
    }
}
