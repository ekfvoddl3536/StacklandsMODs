// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace SmartFactory;

partial class LogicBase
{
    public void OnDestroy()
    {
        try
        {
            DisconnectAll();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        LogicNetworkManager.NextVersion();
    }
}
