// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
using SuperComicLib.Stacklands.Collections;

namespace SmartFactory;

static partial class NetworkGrids
{
    // NOTE:: (KR)
    //  궁극적으로 이 로직에서는 마치 노드가 정렬된 상태로, 입력이 없고 출력만 존재하는 노드에서 출발하여
    //  데이터가 퍼져나가는 연산을 수행하는 것이 목표이다.
    //  
    //  그러나, 이 로직에서는 어떠한 정렬 작업도 노드의 깊이나 순서를 판단하지 않는다.
    //  
    //  즉, 정렬이나 노드 깊이(Depth) 또는 순서(Order)를 명확하게 구분하지 않고서
    //  그러한 역할을 동일하게 수행하면서 노드의 연산을 순서에 신경쓰면서 수행한 것처럼 구현한다.
    //  
    //  이러한 목표를 달성하기 위해서 나는 2개의 스택과 1개의 해시를 준비했다.
    //  
    //  스택은 L1노드와 L2노드 용으로 나뉘며, 여기서 L은 Level의 약자이다.
    //  Level은 Depth와 의미가 비슷하며 Level이 높을수록 의존하는 이전 연산이 많다는 것이다.
    //  L1노드 스택은 현재 진행하는 노드를 L1으로 취급하며, 이전 연산 장치로부터 입력을 받아서
    //  다음 노드로 전파할 값을 소지한 것으로 추정되는 노드들이 모여있는 것이다.
    //  L2노드 스택은 현재 깊이(Level)에서는 처리가 불가능한 노드로 취급되며, 일반적으로 2개 이상의 입력을
    //  가지고 있어서 당장에 연산을 수행할 수 없는 노드를 포함하여 그런 것들이 모여있는 것이다.
    //  
    //  L1노드 스택에서 처리되지 못한 것은 L2노드 스택으로 옮겨지고, 현재 노드의 루프가 끝난 이후 작업을
    //  시작할 수 있을 가능성이 생긴다.
    //  
    //  해시는 스택에 중복된 값을 넣지 않도록 만드는 역할을 하며, 결과적으로 모든 루프가 끝난 후에 해시에는
    //  유효한 연결을 가진 채 네트워크가 구성된 모든 노드가 포함되어야 한다.
    //  
    //  이 구현에서 실제적인 정렬은 존재하지 않지만, 정렬에 가까운 행위가 발생하는 것은 사실이다.
    //  
    //  첫번째 스텝에서는 입력이 존재하지 않고, 출력만이 있는 노드에 대한 연산을 먼저 수행한다.
    //  이 과정에서 L0 노드의 출력이 입력으로 들어가는 L1 노드를 찾고, 이 L1 노드의 끝까지 탐색한다.
    //  만약에 L0 노드에서 이어지는 노드 그룹의 탐색 중에 입력이 하나를 초과하는 다른 노드를 만나면,
    //  L2 노드 스택에 저장하려고 시도한다.
    //  만약, 이 L2 노드를 처음 발견한 경우 현재 입력을 잘 전달했다는 의미로 hasInputValue를 1로 초기화한다.
    //  이 작업은 중요하며 다음에 이 노드에 대한 연산이 시작될 때 모든 값이 유효하게 입력되어 퍼뜨릴 준비가
    //  되었는지 판별하는 데에 사용한다.
    //  이어서, L0 노드에서 출발한 모든 노드 그룹에 대한 연산이 종료되어 모든 노드에서 끝 점 또는 더 이상
    //  연산을 이어나갈 수 없는 노드를 발견하여 L1 노드 스택의 개수가 0개가 되면 다음 L2 노드에 대한 루프를
    //  수행한다.
    //  이 때, 성능을 최적화하기 위해 L2 노드 스택을 수작업으로 비우지 않고, 이미 비어진 L1 노드의 스택과
    //  레퍼런스 스왑하여 교체한다.
    //  L2 노드 스택은 이제 L1 노드 스택으로 취급되며, 다시 앞선 과정이 반복된다.
    //  만약에 노드에 연결된 입력 노드의 개수와 현재 입력받은 개수(hasInputValue)를 비교하여 이 노드의
    //  업데이트가 가능한 것으로 판단되면, 작업이 수행된다.

    /// <summary>
    /// Update network! please
    /// </summary>
    private static void _UpdateCore()
    {
        // L1s --> L2Pred --> L2next

        // L1(lv. 1) 노드 목록
        ref var L1s = ref L1Nodes;

        // var L2pred = L2PredNodes;
        ref var L2next = ref L2Nodes;

        ref var chash = ref currentHash;

        var cards = cachedCards;
        for (int i = 0; i < cards.Length; ++i)
            UpdateNode(cards[i], ref L1s, ref L2next, ref chash);

        for (; ; )
        {
            ref var L1Elem = ref L1s.FirstElementByRef;
            for (ref int i = ref L1s.Count; --i >= 0;)
            {
                var v = Unsafe.Add(ref L1Elem, i);

                Unsafe.Add(ref L1Elem, i) = default;

                if (v._hasInputValueCount >= v._inputConnections)
                    UpdateNode(v, ref L1s, ref L2next, ref chash);
                else
                    L2next.Push(v);
            }

            if (L2next.Count > 0)
            {
                ref var temp = ref L1s;

                L1s = ref L2next;
                L2next = ref temp;
            }
            else
                break;
        }
    }

    private static void UpdateNode(
        LogicBase input,
        ref FastStack<LogicBase> L1s,
        ref FastStack<LogicBase> L2next,
        ref FastHashSet<LogicBase> chash)
    {
        // 노드의 업데이트 수행
        input.NetworkUpdate();

        var outputs = input.OutputNodes;
        for (int j = 0; j < outputs.Length; ++j)
        {
            var item = outputs[j];
            if (item == (object)null)
                continue;

            if (chash.Add(item))
            {
                // initialize
                item._hasInputValueCount = 1;

                // 노드의 입력이 1개 초과이면, 다음 루프까지 지켜본다
                // 그렇지 않으면, 현재 루프에서 처리하려고 시도
                if (item.GetInputConnectionsWithCache() > 1)
                    L2next.Push(item);
                else
                    L1s.Push(item);
            }
            else
                item._hasInputValueCount++;
        }
    }
}
