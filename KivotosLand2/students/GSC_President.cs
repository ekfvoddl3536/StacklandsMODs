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

using System;
using UnityEngine;

namespace KivotosLand
{
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
}