// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands.Collections;

namespace SmartFactory;

partial class LogicBase
{
    // 기본적으로 false
    protected override bool CanHaveCard(CardData otherCard) => false;
    public override bool CanBePushedBy(CardData otherCard) => otherCard is LogicBase;

    protected abstract void SetInputValue(int newInputValue);
    protected abstract int GetNextOutputValue();

    bool IHashedEquatable<LogicBase>.EqualsHash(LogicBase other) => this == other;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal int GetInputConnectionsWithCache()
    {
        _inputConnections = InputConnections;
        return _inputConnections;
    }
}
