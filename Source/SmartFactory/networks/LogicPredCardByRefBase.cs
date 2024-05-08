// MIT License
//
// Copyright (c) 2024. SuperComic (ekfvoddl3535@naver.com)
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

using SuperComicLib.Runtime;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;

namespace SmartFactory
{
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
}
