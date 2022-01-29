using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event Channels/Int Event")]
public class IntEventChannelSO : ScriptableObject {
    public event Action<int> OnEventRaised;
    public void RaiseEvent(int arg) => OnEventRaised?.Invoke(arg);
}
