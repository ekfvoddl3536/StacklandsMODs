// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using fluxiolib;
using System.Reflection;

namespace SmartFactory;

internal static class TypeExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnsafeFieldAccessor GetInstanceFieldAccessor(this Type type, string name) =>
        FluxTool.GetFieldAccessor(GetInstanceFieldInfo(type, name));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldInfo GetInstanceFieldInfo(this Type type, string name)
    {
        const BindingFlags INSTANCE_ALL = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        return type.GetField(name, INSTANCE_ALL);
    }
}
