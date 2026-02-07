// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace KivotosLand;

public sealed class GSC_President : Student
{
    public const float FIXED_UPDATE_DT = 32f;
    public const int MAX_HP = 999;

    private float _timer;

    public override bool HasInventory => false;

    internal override StudentStats BaseStats => new StudentStats(MAX_HP, 998, 30f, 0.5f, 998);

    protected override bool CanHaveEquipable(Equipable equipable) => false;

    public override void UpdateCard()
    {
        _timer += Time.deltaTime * WorldManager.instance.TimeScale;
        if (_timer >= FIXED_UPDATE_DT)
        {
            HealthPoints = Math.Min(HealthPoints + 3, MAX_HP);

            _timer = 0;
        }

        base.UpdateCard();
    }
}