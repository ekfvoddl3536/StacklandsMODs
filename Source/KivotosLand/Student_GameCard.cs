// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace KivotosLand;

partial class Student
{
    private const int PLAY_VOICE_DELAY_MS = 2 * 1000; // 2 seconds

    private static int _lastVoicePlayTick;

    internal static void GameCard_OnStopDragging(GameCard card)
    {
        var audio = AudioManager.me;
        var data = card.CardData;

        if (card.Parent != null)
            audio.PlaySound2D(audio.DropOnStack, UnityEngine.Random.Range(0.8f, 1.2f), 0.3f);
        else
        {
            var group = audio.GetSoundForPickupSoundGroup(data.PickupSoundGroup);
            audio.PlaySound2D(group, UnityEngine.Random.Range(0.8f, 1f), 0.5f);
        }

        for (var child = card.Child; child != null; child = child.Child)
            child.BeingDragged = false;

        data.StoppedDragging();
        card.StackUpdate = true;

        DraggableUnsafeAccessor.constrained_call_StopDragging(card);
    }

    internal static void GameCard_OnStartDragging(GameCard card)
    {
        var data = card.CardData;
        var audio = AudioManager.me;

        var elapsedTick = Environment.TickCount - _lastVoicePlayTick;
        if (elapsedTick > PLAY_VOICE_DELAY_MS && data.PickupSound != null && data.PickupSoundGroup == PickupSoundGroup.Custom)
        {
            var clip_length_ms = (int)(data.PickupSound.length + 0.5f) * 1000;
            _lastVoicePlayTick = Environment.TickCount + (clip_length_ms - PLAY_VOICE_DELAY_MS);

            audio.PlaySound2D(data.PickupSound, 1f, ModOptions.pickupVolume);
        }
        else
        {
            var group = audio.GetSoundForPickupSoundGroup(data.PickupSoundGroup);
            audio.PlaySound2D(group, UnityEngine.Random.Range(1f, 1.2f), 0.5f);
        }

        for (var parent = card.Parent; parent != null; parent = parent.Parent)
        {
            // -- GameCard.NotifyChildDrag(GameCard) --
            parent.removedChild = card;
            // -- end --
        }

        if (card.Parent != null)
            card.SetParent(null);

        for (var child = card.Child; child != null; child = child.Child)
            child.BeingDragged = true;

        card.BounceTarget = null;

        DraggableUnsafeAccessor.constrained_call_StartDragging(card);
    }
}