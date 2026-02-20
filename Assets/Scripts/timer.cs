using System;
using System.Collections;
using UnityEngine;

class Timer : MonoBehaviour
{
    public float _update_time = 1f;
    public bool isRunning = false;

    public Action onTimerUpdate;

    public static Timer Instance { get; private set; }

    private Coroutine _timerCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
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
            onTimerUpdate?.Invoke();
            yield return new WaitForSeconds(tickTime);
        }
    }

    private void Start()
    {
        _timerCoroutine = StartCoroutine(TimeUpdate(_update_time));
    }


    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    void OnDestroy()
    {
        Instance = null;
        StopCoroutine(_timerCoroutine);
    }

}