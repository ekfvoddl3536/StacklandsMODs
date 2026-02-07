// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.Stacklands;

/// <summary>
/// <see cref="ConfigEntry{T}"/> extension
/// </summary>
public static class ConfigEntry_Extensions
{
    /// <summary>
    /// <see cref="ConfigEntry{T}.OnChanged"/> = <paramref name="onChanged"/>
    /// </summary>
    /// <returns><paramref name="entry"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConfigEntry<T> SetOnChanged<T>(this ConfigEntry<T> entry, Action<T> onChanged)
    {
        entry.OnChanged = onChanged;
        return entry;
    }

    /// <summary>
    /// <see cref="ConfigEntry{T}.OnChanged"/> += <paramref name="onChanged"/>
    /// </summary>
    /// <returns><paramref name="entry"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConfigEntry<T> AddOnChanged<T>(this ConfigEntry<T> entry, Action<T> onChanged)
    {
        entry.OnChanged += onChanged;
        return entry;
    }
}
