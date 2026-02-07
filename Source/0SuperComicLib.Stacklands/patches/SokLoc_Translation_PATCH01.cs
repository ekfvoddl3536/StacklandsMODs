// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace SuperComicLib.Stacklands;

[HarmonyPatch(typeof(SokLoc), nameof(SokLoc.Translate), [typeof(string)])]
internal static class SokLoc_Translation_PATCH01
{
    public static bool Prefix(ref string __result, string termId)
    {
        termId = termId.ToLowerCached();

        var instance = SokLoc.instance;
        if (instance != null && 
            instance.CurrentLocSet.TermLookup.TryGetValue(termId, out var sokTerm))
        {
            var text = sokTerm.GetText();
            if (!string.IsNullOrWhiteSpace(text))
            {
                __result = text;
                return false;
            }
        }

        __result =
            SokLoc.FallbackSet.TermLookup.TryGetValue(termId, out var fallback_sokTerm)
            ? fallback_sokTerm.GetText()
            : "---MISSING---";
            
        return false;
    }
}
