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

using System.Threading;

namespace SmartFactory
{
    public sealed class LogicNetworkManager : CardData
    {
        #region static fields
        internal static int IsInitialized;
        
        private static int logicCardsVersion;
        private static int previousLCsV;
        #endregion

        #region connections & line draw
        internal static LogicBase fromCard;
        #endregion

        #region instance fields
        private bool isDeleted;
        private bool isDemoCard;
        #endregion

        internal static void NextVersion() => Interlocked.Increment(ref logicCardsVersion);

        protected override bool CanHaveCard(CardData otherCard) => false;
        public override bool CanBePushedBy(CardData otherCard) => false;

        protected override void Awake()
        {
            isDemoCard = MyGameCard.IsDemoCard;
            if (isDemoCard)
                return;

            // Only 1 card
            if (Interlocked.CompareExchange(ref IsInitialized, 1, 0) != 0)
            {
                ModDebug.Log("Objects created multiple times.");

                isDeleted = true;
                MyGameCard.DestroyCard(true);
                return;
            }

            logicCardsVersion = 0;
            previousLCsV = 0;

            NetworkGrids.Initialize();
            NetworkGrids.UpdateCache();
        }

        public void OnDestroy()
        {
            if (!isDemoCard && !isDeleted)
                IsInitialized = 0;
        }

        public static void OnUpdate()
        {
            if (IsInitialized == 0)
                return;

            if (logicCardsVersion != previousLCsV)
                NetworkGrids.UpdateCache();

            NetworkGrids.Update();
        }

        public static void SelectCard(LogicBase card)
        {
            if (fromCard == (object)null)
            {
                fromCard = card;
                return;
            }

            if (fromCard != (object)card)
                if (card.HasInputNode(fromCard))
                    card.DisconnectFrom(fromCard);
                else if (fromCard.CanOutputsConnect && card.CanConnectFrom(fromCard))
                {
                    card.ConnectFrom(fromCard);
                    NetworkArrows.DrawArrow(fromCard, card);
                }

            fromCard = null!;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // private static void ShowNotification(string titleTerm, string textTerm)
        // {
        //     var title = SokLoc.Translate(titleTerm);
        // 
        //     if (connTryCount > 0)
        //         title += $" ({connTryCount})";
        // 
        //     GameScreen.instance.AddNotification(
        //         title,
        //         SokLoc.Translate(textTerm));
        // }
    }
}
