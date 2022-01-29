using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] PlayerStateChannelSO PlayerStateChannel;
    [SerializeField] CinemachineVirtualCamera MainCam;
    [SerializeField] CinemachineVirtualCamera DramaCam;

    private void Start() {
        MainCam.enabled = true;
        DramaCam.enabled = false;
    }

    private void OnEnable() {
        PlayerStateChannel.ExitingState += OnStateExiting;
        PlayerStateChannel.EnteredState += OnStateEntered;
    }

    private void OnDisable() {
        PlayerStateChannel.ExitingState -= OnStateExiting;
        PlayerStateChannel.EnteredState -= OnStateEntered;
    }

    private void OnStateEntered(PlayerState state) {
        if(state.GetType() == typeof(PlayerWaterDrawState)) {
            MainCam.enabled = false;
            DramaCam.enabled = true;
        }
    }

    private void OnStateExiting(PlayerState state) {
        if (state.GetType() == typeof(PlayerWaterDrawState)) {
            MainCam.enabled = true;
            DramaCam.enabled = false;
        }
    }
}
