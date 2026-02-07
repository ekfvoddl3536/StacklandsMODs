// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace SmartFactory;

public sealed class ConstantBuilder : LogicBase
{
    public const string MY_ACTION_ID = "create_constant";

    private static string global_statusOverride;

    [ExtraData("nextProduce")]
    public int nextConstValue;

    private TimerAction onEndTimer;

    private bool allowUpdate;

    protected override void Awake()
    {
        if (global_statusOverride == null)
        {
            var text = SokLoc.Translate(StringTerms.label_constbuilder_status);

            Interlocked.CompareExchange(ref global_statusOverride, text, null);
        }

        if (Inputs == null || Inputs.Length != 1)
            Inputs = new LogicType[1] { LogicType.Any };

        Outputs = Array.Empty<LogicType>();

        onEndTimer = CreateNewConstant;

        base.Awake();
    }

    // must computefield, and no variables
    public override bool CanConnectFrom(LogicBase inputCard) => 
        inputCard is ComputeField field && 
        field.CheckIfHasAnyVariablesImmediate() == false;

    protected override void SetInputValue(int newInputValue)
    {
        // @DISABLE_NO_CHECK
        // var field = (ComputeField)InputNodes.refdata();
        var field = (ComputeField)InputNodes[0];

        allowUpdate = field.StateFlags == 0;
        if (allowUpdate == false)
            MyGameCard.CancelAnyTimer();
        else
        {
            // 같은 값이 입력됐다면, 아무것도 하지 않는다. (타이머 계속 진행)
            if (nextConstValue == newInputValue)
                return;

            MyGameCard.CancelAnyTimer();

            nextConstValue = newInputValue;
        }
    }

    protected override int GetNextOutputValue() => 0;

    protected override void OnUpdateCard()
    {
        // has input node
        if (allowUpdate && CanInputsConnect == false)
        {
            var gc = MyGameCard;
            if (gc.Parent != (object)null)
                gc.CancelAnyTimer();
            else if (gc.TimerRunning == false)
                gc.StartTimer(60f, onEndTimer, global_statusOverride, MY_ACTION_ID);
        }

        CheckNodeLength();
    }

    private void CreateNewConstant()
    {
        // 다음 업데이트는 다음 값이 입력된 이후로 미룬다
        allowUpdate = false;

        var card = (ConstantCard)WorldManager.instance.CreateCard(
            transform.position + Vector3.up * 0.2f, 
            MyModCardIDs.sf_number_constant,
            true,
            false,
            true);

        card.constValue = nextConstValue;

        card.UpdateDisplayName();
    }
}
