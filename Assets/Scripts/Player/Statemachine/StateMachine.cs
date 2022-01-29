using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {
    private PlayerController player;
    private PlayerStateChannelSO playerStateChannel;
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerController player, PlayerState startingState, PlayerStateChannelSO playerStateChannel) {
        CurrentState = startingState;
        this.player = player;
        this.playerStateChannel = playerStateChannel;
        player.CurrentState = CurrentState.ToString();
    }

    public void ChangeState(PlayerState newState) {
        if (CurrentState.ExitingState)
            return;

        playerStateChannel.TriggerExitingState(CurrentState);
        CurrentState.Exit();
        CurrentState = newState;
        player.CurrentState = CurrentState.ToString();
        CurrentState.Enter();
        playerStateChannel.TriggerEnteredState(CurrentState);
    }
}
