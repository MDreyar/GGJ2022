using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System.Linq;

public class MusicManager : MonoBehaviour {
    private EventInstance instance;

    public EventReference fmodEvent;

    [SerializeField]
    private VoidEventChannelSO moistSuckySucky;

    [SerializeField]
    private FloatEventChannelSO fireProximity;

    [SerializeField]
    private float circlePercentage;

    [SerializeField]
    private float why;

    private Coroutine currentRoutine;

    void Start() {
        moistSuckySucky.OnEventRaised += SetMoistLevel;
        fireProximity.OnEventRaised += FireProximityLevel;
        instance = RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
        instance.setParameterByName("Moist Level", 1);
    }

    private void OnDestroy() {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void SetMoistLevel() {
        instance.getParameterByName("Moist Level", out var moistLevel);
        StartCoroutine(MathHelper.SmoothTowards(moistLevel, moistLevel - circlePercentage, 1.5f,
            newMoistLevel => instance.setParameterByName("Moist Level", newMoistLevel)));
    }

    void FireProximityLevel(float proximity) {
        if (currentRoutine != null) {
            StopCoroutine(currentRoutine);
        }
        instance.getParameterByName("Fire Proximity", out var fireProximity);
        //instance.setParameterByName("Fire Proximity", (1/proximity).Map(0,0.5f,0,1f));
        why = proximity == 0 ? 0 : (1 / proximity).Map(0, 0.5f, 0, 1f);
        //StartCoroutine(MathHelper.SmoothTowards(fireProximity, why, 1f, newFireProximity => instance.setParameterByName("Fire Proximity", newFireProximity)));
        instance.setParameterByName("Fire Proximity", Mathf.MoveTowards(fireProximity, why, 0.1f * Time.deltaTime));
    }

    /*
    void Update()
    {
        instance.setParameterByName("Fire Proximity", fireProximity);
    }*/
}
