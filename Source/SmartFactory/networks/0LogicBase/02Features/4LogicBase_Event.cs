// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine.InputSystem;

namespace SmartFactory;

partial class LogicBase
{
    public override void Clicked()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.ctrlKey.isPressed)
            LogicNetworkManager.SelectCard(this);
        else if (keyboard.tKey.isPressed)
            DisconnectAll();
    }

    public virtual bool CanConnectFrom(LogicBase inputCard) =>
        inputCard.MyGameCard.Parent == false &&
        CanInputsConnect &&
        _GetDistance(inputCard, MyGameCard.transform.position) <= ModOptions.maxConnLen;
}
