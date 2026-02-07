// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

public class SmartControl : HeavyFoundation, ISmartCard
{
    public override void UpdateCard()
    {
        // disallow have parent
        if (MyGameCard.Parent)
            MyGameCard.RemoveFromStack();

        base.UpdateCard();
    }

    void ISmartCard.SetOperational(bool isOperational)
    {
        var child = MyGameCard.Child;
        for (; child; child = child.Child)
        {
            child.enabled = isOperational;
            child.CardData.enabled = isOperational;
        }
    }
}
