// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
namespace SmartFactory;

public class LogicCounter : LogicPredCardByRefBase
{
    protected override void Awake()
    {
        if (Outputs == null || Outputs.Length != 1)
            Outputs = new LogicType[1] { LogicType.Any };

        Inputs = Array.Empty<LogicType>();

        base.Awake();
    }


    protected override void OnUpdateCard()
    {
        if (_target != (object)null)
            NetworkArrows.DrawArrow(_target, this);

        base.OnUpdateCard();
    }

    protected override void SetInputValue(int newInputValue) { }
    protected override int GetNextOutputValue() => 
        _target != (object)null
        ? OnCount(_target)
        : 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static int OnCount(CardData card)
    {
        var root = card.MyGameCard;

        int sum = 0;
        for (; root; root = root.Child)
            sum += 1 + _ItemCount(root.CardData);

        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static int _ItemCount(CardData c) =>
        c is ResourceChest rc
        ? rc.ResourceCount
        : c is Chest cs
        ? cs.CoinCount
        : 0;
}
