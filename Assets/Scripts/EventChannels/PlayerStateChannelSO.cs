using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event channels/Player statemachine events")]
public class PlayerStateChannelSO : ScriptableObject
{
    public event Action<PlayerState> ExitingState;
    public event Action<PlayerState> EnteredState;

    public void TriggerExitingState(PlayerState state) => ExitingState?.Invoke(state);
    public void TriggerEnteredState(PlayerState state) => EnteredState?.Invoke(state);
}
