// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands;

namespace SmartFactory;

public sealed unsafe class ModLoad : Mod
{
    internal static ConfigFile CurrentConfig;

    public override void Ready()
    {
        Logger.Log(nameof(SmartFactory) + " MOD Loading... by 'SuperComic (ekfvoddl3535@naver.com)'");

        try
        {
            LogPhase(1, "Load fallback terms");
            this.LoadFallbackTerms();


            LogPhase(2, "Load dependencies");
            this.PatchAllWithDependencies(Harmony, false);


            LogPhase(3, "Patch");
            Harmony.PatchAll();


            LogPhase(4, "Read MOD Options");
            LoadMODOptions();


            LogPhase(5, "Post Load");
            PostLoad();

            Logger.Log(nameof(SmartFactory) + " MOD Loaded!");
        }
        catch (Exception e)
        {
            LogFail(e);
        }
    }

    private void LoadMODOptions()
    {
        CurrentConfig = Config;
        ModOptions.Load(Logger);
        CurrentConfig = null;
    }

    private void PostLoad()
    {
        WorldManager.instance.gameObject.AddComponent<MOD_SMARTFACTORY_BY_SUPERCOMIC_MAIN_SYSTEM>();

        ModPostLoad.Ready();
    }

    private void LogPhase(int step, string title) =>
        Logger.Log(nameof(SmartFactory) + $" / Phase {step}: {title}");

    private void LogFail(Exception e) =>
        Logger.LogException(nameof(SmartFactory) + " MOD Load FAIL! -> " + e.ToString());
}
