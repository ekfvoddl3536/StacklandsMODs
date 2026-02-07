// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

public class LogicSwitch : LogicPredCardByRefBase
{
    [ExtraData("switchValue")]
    public bool m_ison;
    
    protected override void Awake()
    {
        if (Inputs == null || Inputs.Length != 1)
            Inputs = new LogicType[1] { LogicType.Any };
    
        Outputs = Array.Empty<LogicType>();
    
        base.Awake();
    }

    protected override void OnUpdateCard()
    {
        if (_target != (object)null)
            NetworkArrows.DrawArrow(this, _target);

        base.OnUpdateCard();
    }
    
    protected override int GetNextOutputValue() => 0;
    protected override void SetInputValue(int newInputValue)
    {
        var v = newInputValue != 0;
    
        if (m_ison != v)
        {
            m_ison = v;
    
            if (_target != (object)null)
                SetOperation(_target, v);
        }
    }
    
    protected override void OnUnlinkedTarget(CardData other)
    {
        m_ison = true;
        SetOperation(other, true);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetOperation(CardData c, bool ison)
    {
        for (var g = c.MyGameCard; g != (object)null; g = g.Child)
        {
            g.CardData.enabled = ison;
            g.enabled = ison;
        }
    }
}
