using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{

    public float size;
    public float growDuration;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GrowEffect());
    }

    IEnumerator GrowEffect() {
        var startTime = Time.time;
        while (transform.localScale.x < size) {
            var newScale = Mathf.Lerp(1f, size, Time.time.Map(startTime, startTime + growDuration, 0.1f, 1f));
            transform.localScale = new Vector3(newScale, newScale);
            yield return null;
        }
    }
}
