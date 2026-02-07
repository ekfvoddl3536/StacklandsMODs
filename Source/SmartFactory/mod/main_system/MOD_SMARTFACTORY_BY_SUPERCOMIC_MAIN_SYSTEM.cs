// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

internal sealed class MOD_SMARTFACTORY_BY_SUPERCOMIC_MAIN_SYSTEM : UnityEngine.MonoBehaviour
{
    public static float delayedTime;

    public void LateUpdate()
    {
        delayedTime += UnityEngine.Time.deltaTime;
        if (delayedTime >= ModOptions.updateInterval)
        {
            delayedTime = 0;

            LogicNetworkManager.OnUpdate();
            
            NetworkArrows.OnUpdateArrowToMouse(LogicNetworkManager.fromCard);
        }
    }
}
