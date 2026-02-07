// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)
#nullable enable

using System.Reflection;

namespace KivotosLand;

internal static class DraggableUnsafeAccessor
{
    private static readonly nint _fpStopDragging;
    private static readonly nint _fpStartDragging;

    internal static readonly bool _isSupported;

    static DraggableUnsafeAccessor()
    {
        const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var t = typeof(Draggable);

        var method_StopDragging = t.GetMethod(nameof(Draggable.StopDragging), FLAGS, null, [], null);
        var method_StartDragging = t.GetMethod(nameof(Draggable.StartDragging), FLAGS, null, [], null);

        // checking method signature (delegate* managed<Draggable, void>)
        if (method_StopDragging is null || method_StartDragging is null ||
            method_StopDragging.ReturnType != typeof(void) || method_StartDragging.ReturnType != typeof(void))
        {
            // if method not found. stop processing
            _isSupported = false;
            return;
        }

        _fpStopDragging = method_StopDragging.MethodHandle.GetFunctionPointer();
        _fpStartDragging = method_StartDragging.MethodHandle.GetFunctionPointer();
        _isSupported = true;
    }

    public static void Initialize(ModLogger logger)
    {
        if (!_isSupported)
        {
            logger.LogWarning($"[{nameof(DraggableUnsafeAccessor)}] Failed to configure constrained_call. Voice playback will be disabled.");
        }
    }

    public static unsafe void constrained_call_StartDragging(Draggable @this)
    {
        ((delegate* managed<Draggable, void>)_fpStartDragging)(@this);
    }

    public static unsafe void constrained_call_StopDragging(Draggable @this)
    {
        ((delegate* managed<Draggable, void>)_fpStopDragging)(@this);
    }
}
