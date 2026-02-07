// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace SuperComicLib.Stacklands;

[HarmonyPatch(typeof(ModManager), "ReadyUpMods")]
internal static class ModManager_ReadyUpMods_PATCH01
{
    public static void Postfix()
    {
        Mod_Extensions._assemblies.Clear();
        Mod_Extensions._assemblies = null;

        GC.Collect(0);
    }
}
