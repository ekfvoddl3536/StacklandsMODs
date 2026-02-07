// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

public sealed class ComputeValueViewCard : CardData, IComputable
{
    private int viewValue;
    private int previousValue;

    public override void UpdateCardText()
    {
        if (viewValue != previousValue)
        {
            previousValue = viewValue;

            nameOverride = "VIEW: " + viewValue.ToString();
        }
    }

    protected override bool CanHaveCard(CardData otherCard) => ConstantCard._CanHaveCard(otherCard);

    ComputeType IComputable.ComputeType => ComputeType.View;
    int IComputable.StackPop => 0;
    int IComputable.ComputeValue(in ComputeStack<int> stack)
    {
        viewValue = stack.PeekOrDefault();
        return 0;
    }
}
