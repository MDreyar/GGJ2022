using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event Channels/Void Event")]
public class VoidEventChannelSO : ScriptableObject
{
    public event Action OnEventRaised;
    public void RaiseEvent() => OnEventRaised?.Invoke();
}
