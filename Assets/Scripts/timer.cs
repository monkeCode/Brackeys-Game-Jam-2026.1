using System;
using System.Collections;
using UnityEngine;

class Timer: MonoBehaviour
{
    public float time = 0f;
    public bool isRunning = false;

    public Action<float> onTimerUpdate;

    public static Timer Instance { get; private set; }

    private Coroutine _timerCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator TimeUpdate(float tickTime)
    {
        while (isRunning)
        {
            time += tickTime;
            onTimerUpdate?.Invoke(time);
            yield return new WaitForSeconds(tickTime);
        }
    }

    private void Start()
    {
        _timerCoroutine =  StartCoroutine(TimeUpdate(1f));
    }


    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        time = 0f;
        isRunning = false;
    }

    void OnDestroy()
    {
        Instance = null;
        StopCoroutine(_timerCoroutine);
    }

}