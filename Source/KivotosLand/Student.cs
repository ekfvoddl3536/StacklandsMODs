// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using System.Reflection;
using UnityEngine;

namespace KivotosLand;

public abstract class Student : TeenageVillager
{
    protected internal static FieldInfo _AttackSpecialHit;
    protected internal static PerformSpecialHitDelegate _PerformSpecialHit;
    protected internal static ShowHitTextDelegate _ShowHitText;

    protected internal SpriteRenderer _fullColorIconRenderer;
    protected internal MaterialPropertyBlock _propBlock;

    protected internal int _flags;

    internal abstract StudentStats BaseStats { get; }

    public override bool HasInventory => true;

    protected override bool CanHaveCard(CardData otherCard)
    {
        if (otherCard.Id == Cards.royal_crown)
            return false;
        else if (otherCard.MyCardType == CardType.Resources || otherCard is Student)
            return true;

        return 
            otherCard.MyCardType == CardType.Equipable
            ? CanHaveEquipable((Equipable)otherCard)
            : otherCard is Food food
            ? food.CanBePlacedOnVillager
            : otherCard.Id == Cards.naming_stone;
    }

    protected virtual bool CanHaveEquipable(Equipable equipable) => equipable.Id != Cards.blunderbuss;

    protected override void Awake()
    {
        if (_flags == 0)
            Initialize();
    }

    internal void Initialize()
    {
        _flags = 1;

        if (ModOptions.onFCR)
        {
            _propBlock = new MaterialPropertyBlock();

            var iconRenderer = MyGameCard.IconRenderer;

            var go1 = new GameObject("_FullColorRenderer");
            go1.transform.SetParent(iconRenderer.transform.parent);

            iconRenderer.gameObject.SetActive(false);

            var renderer = go1.AddComponent<SpriteRenderer>();
            var tr = renderer.transform;
            tr.rotation = iconRenderer.transform.rotation;
            tr.localScale = new Vector3(0.031f, 0.031f);
            tr.localPosition = new Vector3(0, -0.025f, -0.0078125f);

            renderer.material = new Material(GlobalValues.shader_spriteDefault);
            renderer.color = Color.white;
            renderer.sprite = Icon;
            renderer.sortingOrder = 1;

            MyGameCard.SpecialIcon.sortingOrder = 2;
            MyGameCard.SpecialText.sortingOrder = 2;

            _fullColorIconRenderer = renderer;
        }

        CurrentAttackType = BaseAttackType = AttackType.Ranged;
        Value = -1;

        InitStats();
    }

    public override int GetRequiredFoodCount() => 1;

    public override void UpdateCardText()
    {
        nameOverride = SokLoc.Translate(NameTerm);

        if (BaseCombatStats != null && BaseCombatStats.SpecialHits != null)
        {
            var descText =
                SokLoc.Translate(DescriptionTerm) +
                $"\n\n<i>{GetCombatableDescription()}</i>";

            if (AdvancedSettingsScreen.AdvancedCombatStatsEnabled || GameCanvas.instance.CurrentScreen is CardopediaScreen)
                descText += $"\n\n<i>{GetCombatableDescriptionAdvanced()}</i>";

            descriptionOverride = descText;
        }

        var gc = MyGameCard;
        if (gc != null && gc.CardConnectorChildren.Count > 0 && gc.IsHovered)
        {
            descriptionOverride = SokLoc.Translate(DescriptionTerm);
            descriptionOverride += $"\n\n<i>{GetConnectorInfoString(gc)}</i>";
        }
    }

    public override void PerformAttack(Combatable target, Vector3 attackPos)
    {
        if (target == null)
            return;

        StunTimer = 0;
        if (AttackIsHit)
        {
            int num = Mathf.Clamp(GetDamage(target), 0, int.MaxValue);

            var attackSpecialHit = (SpecialHit)_AttackSpecialHit.GetValue(this);
            if (attackSpecialHit != null && attackSpecialHit.HitType != SpecialHitType.None)
            {
                if (!target.HasStatusEffectOfType<StatusEffect_Invulnerable>())
                    _PerformSpecialHit.Invoke(this, attackSpecialHit, target, num);
                else
                    target.Damage(num);

                _ShowHitText.Invoke(this, this, target, attackPos, AttackIsHit, num, new SpecialHitType?(attackSpecialHit.HitType));
            }
            else
            {
                target.Damage(num);
                _ShowHitText.Invoke(this, this, target, attackPos, AttackIsHit, num, null);
            }
        }
        else
            _ShowHitText.Invoke(this, this, target, attackPos, AttackIsHit, -1, null);
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);

        StunTimer = 0;
        GameCamera.instance.Screenshake = 0.05f;
    }

    internal void InitStats()
    {
        var stats = BaseStats;

        HealthPoints = stats.HealthPoints;

        var cst = BaseCombatStats;
        cst.MaxHealth = stats.HealthPoints;
        cst.AttackDamage = stats.AttackDamage;
        cst.AttackSpeed = stats.AttackSpeed;
        cst.HitChance = stats.HitChance;
        cst.Defence = stats.Defence;

        cst.SpecialHits ??= new List<SpecialHit>();
    }
}