// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands;
using UnityEngine;

namespace SmartFactory;

internal static class ModOptions
{
    public static readonly float updateInterval;
    public static readonly bool onDebug;

    public static readonly float maxConnLen;

    public static readonly int maxComputeStack;

    // WIP
    // public static readonly bool onBatchUpdates;
    // public static readonly int maxBatchSize;

    static ModOptions()
    {
        // 25fps
        const int FPS = 25;

        var config = ModLoad.CurrentConfig;

        updateInterval = config.GetValue(nameof(updateInterval), 1f / FPS, true);
        // clamp
        updateInterval = Mathf.Clamp(updateInterval, 0f, 1f);

        // onDebug = config.GetValue(nameof(onDebug), false, true);
        onDebug = config.GetValue(nameof(onDebug), true, true);

        maxConnLen = config.GetValue(nameof(maxConnLen), 3.5f, true);
        maxConnLen = Mathf.Clamp(maxConnLen, 0, 20f);
        maxConnLen *= maxConnLen;

        maxComputeStack = config.GetValue(nameof(maxComputeStack), 30, true);
        maxComputeStack = Mathf.Clamp(maxComputeStack, 8, 120);

        // WIP
        // onBatchUpdates = config.GetValue(nameof(onBatchUpdates), false, true);
        // 
        // maxBatchSize = config.GetValue(nameof(maxBatchSize), 1_000, true);
        // maxBatchSize = Mathf.Max(maxBatchSize, 100);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Load(ModLogger logger)
    {
        if (onDebug)
            logger.LogWarning(" ======!!! DEBUG FEATURES ENABLED !!!====== ");

        // WIP
        // if (onBatchUpdates)
        // {
        //     logger.LogWarning(" Experimental feature has been activated.");
        //     logger.LogWarning(" Max Batch Size: " + maxBatchSize);
        // }
    }
}
