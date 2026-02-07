// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable CS1591 // XML comment
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace SuperComicLib.Stacklands;

public static class Mod_Extensions
{
    internal static HashSet<string> _assemblies;

    public static void PatchAllWithDependencies(this Mod mod, Harmony harmony, bool forceLoadAssemblies = false)
    {
        var patchedDLLNames = _assemblies ??= [];

        var loadedAssemblies =
            forceLoadAssemblies
            ? []
            : AppDomain.CurrentDomain.GetAssemblies();

        var name = Path.GetFileName(mod.GetType().Assembly.Location);
        foreach (var dllPath in Directory.EnumerateFiles(mod.Path, "*.dll"))
        {
            if (dllPath.EndsWith(name))
                continue;

            var fileName = Path.GetFileName(dllPath);
            if (patchedDLLNames.Contains(fileName))
            {
                Debug.Log("Already patched assembly -> " + fileName);
                continue;
            }

            try
            {
                var assembly = FindAssembly(loadedAssemblies, fileName);
                if (assembly == null)
                    if (!forceLoadAssemblies)
                    {
                        Debug.LogWarning("The assembly is not loaded, and since forced loading is not enabled, it is skipped.");
                        continue;
                    }
                    else
                        assembly = Assembly.LoadFrom(dllPath);

                harmony.PatchAll(assembly);

                patchedDLLNames.Add(fileName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    private static Assembly FindAssembly(Assembly[] loaded, string name)
    {
        for (int i = 0; i < loaded.Length; ++i)
            if (loaded[i].Location.EndsWith(name))
                return loaded[i];

        return null;
    }

    /// <summary>
    /// Load a string for the default language 'English'.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LoadFallbackTerms(this Mod mod, string relative_path = null, bool disableWarning = false)
    {
        var locPath = Path.Combine(mod.Path, relative_path ?? "localization.tsv");
        if (!File.Exists(locPath))
            return;

        var tableFromTsv = SokLoc.ParseTableFromTsv(File.ReadAllText(locPath));

        var fallback = SokLoc.FallbackSet;

        var langCol_index = ColumnIndexOf(tableFromTsv[0], fallback.Language);
        if (langCol_index < 0)
        {
            Debug.LogError("No fallback terms -> " + fallback.Language);
            return;
        }

        var lookup = fallback.TermLookup;
        var allTerms = fallback.AllTerms;
        for (int i = 1; i < tableFromTsv.Length; ++i)
        {
            var row = tableFromTsv[i];

            var term = row[0].Trim().ToLower();
            var text = row[langCol_index];

            if (string.IsNullOrEmpty(term))
                continue;

            var sokTerm = new SokTerm(fallback, term, text);

            if (lookup.ContainsKey(term))
            {
                if (!disableWarning)
                    Debug.LogError($"Term '{term}' has been found more than once in the localisation sheet. Using last time in sheet.");

                lookup[term] = sokTerm;

                allTerms.RemoveAll(tmp_x => tmp_x.Id == term);
                allTerms.Add(sokTerm);
            }
            else
            {
                allTerms.Add(sokTerm);
                lookup.Add(term, sokTerm);
            }
        }
    }

    private static int ColumnIndexOf(string[] langs, string value)
    {
        for (int i = 0; i < langs.Length; ++i)
            if (langs[i] == value)
                return i;

        return -1;
    }
}
