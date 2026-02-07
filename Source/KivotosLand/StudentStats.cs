// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace KivotosLand;

[StructLayout(LayoutKind.Explicit)]
internal readonly ref struct StudentStats
{
    // higher the better
    [FieldOffset(sizeof(long) * 0)]
    public readonly int HealthPoints;

    // higher the better
    [FieldOffset(sizeof(long) * 1)]
    public readonly int AttackDamage;

    // lower the better
    [FieldOffset(sizeof(long) * 2)]
    public readonly float AttackSpeed;

    // [0..1] higher the better
    [FieldOffset(sizeof(long) * 3)]
    public readonly float HitChance;

    // higher the better
    [FieldOffset(sizeof(long) * 4)]
    public readonly int Defence;

    public StudentStats(int hp, int atkDmg = 1, float atkSpd = 3.5f, float hit = 0.5f, int def = 1)
    {
        HealthPoints = hp;
        AttackDamage = atkDmg;
        AttackSpeed = atkSpd;
        HitChance = hit;
        Defence = def;
    }
}