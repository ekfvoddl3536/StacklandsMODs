// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace SuperComicLib.Stacklands;

/// <summary>
/// <see cref="ConfigFile"/> extensions
/// </summary>
public static class ConfigFile_Extensions
{
    /// <summary>
    /// Reads the <see cref="ConfigEntry{T}"/> of the configuration value with the specified name from the <see cref="ConfigFile"/>.
    /// <para/>
    /// The name and tooltip of the UI are retrieved by TermId with the following pattern:<br/>
    /// <br/>
    /// name     : <c>"op_" + <paramref name="name"/> + "_name"</c><br/>
    /// tooltipe : <c>"op_" + <paramref name="name"/> + "_tooltip"</c><br/>
    /// <br/>
    /// If a TermId in the current language cannot be found, English is the default.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConfigEntry<T> GetEntry<T>(this ConfigFile conf, string name, T defaultValue, bool restartAfterChanged = false)
    {
        var term = "op_" + name;

        var entry = conf.GetEntry<T>(name, defaultValue, new ConfigUI
        {
            RestartAfterChange = restartAfterChanged,
            NameTerm = term + "_name",
            TooltipTerm = term + "_tooltip"
        });

        if (!SokLoc.instance.CurrentLocSet.ContainsTerm(entry.UI.NameTerm))
        {
            Debug.LogWarning("The provided string is not available in the current language. Attempting to substitute in English...");

            var fallback = SokLoc.FallbackSet;

            var ui = entry.UI;
            ui.Name = fallback.TranslateTerm(ui.NameTerm);
            ui.Tooltip = fallback.TranslateTerm(ui.TooltipTerm);

            ui.NameTerm = null;
            ui.TooltipTerm = null;
        }

        return entry;
    }

    /// <summary>
    /// Reads the <see cref="ConfigEntry{T}.Value"/> of the configuration value with the specified name from the <see cref="ConfigFile"/>.
    /// <para/>
    /// The name and tooltip of the UI are retrieved by TermId with the following pattern:<br/>
    /// <br/>
    /// name     : <c>"op_" + <paramref name="name"/> + "_name"</c><br/>
    /// tooltipe : <c>"op_" + <paramref name="name"/> + "_tooltip"</c><br/>
    /// <br/>
    /// If a TermId in the current language cannot be found, English is the default.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValue<T>(this ConfigFile conf, string name, T defaultValue, bool restartAfterChanged = false) =>
        GetEntry(conf, name, defaultValue, restartAfterChanged).Value;
}
