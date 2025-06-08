using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SlowTime(float scale, float duration)
    {
        if (Instance == null)
        {
            Debug.LogWarning("TimeManager.Instance is null");
            return;
        }
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