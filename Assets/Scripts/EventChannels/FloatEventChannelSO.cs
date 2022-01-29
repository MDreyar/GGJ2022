using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event Channels/Float Event")]
public class FloatEventChannelSO : ScriptableObject
{
    public event Action<float> OnEventRaised;
    public void RaiseEvent(float arg) => OnEventRaised?.Invoke(arg);
}
