// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Runtime;

namespace SmartFactory;

public abstract class LogicPredCardByRefBase : LogicBase, IPredCardByRefLogic
{
    protected CardData? _target;

    [ExtraData("targetID")]
    public string m_targetUniqueId;

    protected override void OnDisconnectedAll() => _target = null;

    bool IPredCardByRefLogic.TryLinkByRef(CardData other)
    {
        if (_target != (object)null)
        {
            if (_target == (object)other)
            {
                OnUnlinkedTarget(other);

                LogicNetworkManager.fromCard = null;

                return true;
            }
        }
        else if (CanLink(other))
        {
            OnLinkedTarget(other);

            LogicNetworkManager.fromCard = null;

            return true;
        }
        
        return false;
    }

    protected override void CheckNodeLength()
    {
        if (IsDraggingMe())
        {
            ConnectionUpdate(InputNodes, Inputs);

            ConnectionUpdate(OutputNodes, Outputs);

            if (_target != (object)null &&
                _GetDistance(_target, MyGameCard.transform.position) > ModOptions.maxConnLen)
                OnUnlinkedTarget(_target);
        }
    }

    protected virtual void OnLinkedTarget(CardData other)
    {
        var root = other.MyGameCard;
        while (root.Parent != (object)null)
            root = root.Parent;

        _target = root.CardData;
    }
    protected virtual void OnUnlinkedTarget(CardData other) => _target = null;

    protected override void OnSave() => m_targetUniqueId = GetUniqueId(_target);
    protected override void OnLoad() => FromUniqueId(m_targetUniqueId, ref _target);

    protected bool CanLink(CardData other) => 
        !(other is LogicNetworkManager) &&
        _GetDistance(other, MyGameCard.transform.position) <= ModOptions.maxConnLen;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static string GetUniqueId(CardData? card) =>
        card != (object)null 
        ? card.UniqueId
        : string.Empty;

    protected static void FromUniqueId<T>(string? id, ref T? item) where T : CardData
    {
        if (string.IsNullOrEmpty(id))
            return;

        var gc =
            WorldManager.instance.AllCards
            .AsParallel()
            .AsUnordered()
            .FirstOrDefault(x => x!.CardData!.UniqueId == id);

        if (gc == (object)null)
        {
            UnityEngine.Debug.LogError($"Can't find unique id = '{id}'");
            item = null;
        }
        else
            item = XUnsafe.As<T>(gc.CardData);
    }
}
