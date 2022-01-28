using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {
    protected PlayerController player;
    protected StateMachine stateMachine;
    protected PlayerData data;

    public bool ExitingState { get; private set; }

    public PlayerState(PlayerController player, StateMachine stateMachine, PlayerData data) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.data = data;
    }

    public virtual void Enter() {
        ExitingState = false;
       // Debug.Log("Entering state " + this.ToString());
    }

    public virtual void Exit() {
        ExitingState = true;
    }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }
}
