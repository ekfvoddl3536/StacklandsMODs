// MIT License
//
// Copyright (c) 2023. SuperComic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Reflection;
using UnityEngine;

namespace KivotosLand
{
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
            var descText =
                SokLoc.Translate(DescriptionTerm) + 
                $"\n\n<i>{GetCombatableDescription()}</i>";

            if (AdvancedSettingsScreen.AdvancedCombatStatsEnabled || GameCanvas.instance.CurrentScreen is CardopediaScreen)
                descText += $"\n\n<i>{GetCombatableDescriptionAdvanced()}</i>";

            descriptionOverride = descText;

            nameOverride = SokLoc.Translate(NameTerm);
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
        }
    }
}