// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace SuperComicLib.Stacklands;

[HarmonyPatch(typeof(SokLoc), nameof(SokLoc.Translate), [typeof(string), typeof(LocParam[])])]
internal static class SokLoc_Translation_PATCH02
{
    public static bool Prefix(ref string __result, string termId, LocParam[] locParams)
    {
        termId = termId.ToLowerCached();

        var instance = SokLoc.instance;
        if (instance != null)
        {
            var locSet = instance.CurrentLocSet;
            if (locSet.TermLookup.TryGetValue(termId, out var sokTerm))
            {
                var text = locSet.FillInTerm(sokTerm, locParams);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    __result = text;
                    return false;
                }
            }
        }

        var fallback = SokLoc.FallbackSet;
        __result =
            fallback.TermLookup.TryGetValue(termId, out var fallback_sokTerm)
            ? fallback.FillInTerm(fallback_sokTerm, locParams)
            : "---MISSING---";

        return false;
    }
}
