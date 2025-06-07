using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    void Awake() => Instance = this;

    public void SlowTime(float scale, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowRoutine(scale, duration));
    }

    IEnumerator SlowRoutine(float scale, float duration)
    {
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}
