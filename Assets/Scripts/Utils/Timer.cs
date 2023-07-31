using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField]
    private float defaultTime = 10.0f;

    [Space(20)]
    [Header("Reference")]
    [SerializeField]
    private Image[] dissolveImages;

    [SerializeField]
    private GameObject UIObject;

    public bool IsRunning { get; private set; } = false;

    public float DefaultTime => defaultTime;

    public float RemainingTime { get; private set; }

    public event Action<Timer> OnTimerStop;

    private void Start()
    {
        UIObject.SetActive(false);
        ResetTimer();
    }

    private void FixedUpdate()
    {
        if (!IsRunning)
        {
            return;
        }

        if (RemainingTime <= 0)
        {
            StopTimer();

            return;
        }

        RemainingTime -= Time.deltaTime;
        if (dissolveImages != null && dissolveImages.Length > 0)
        {
            int index = Math.Max(0, dissolveImages.Length - Mathf.FloorToInt(RemainingTime / dissolveImages.Length) - 1);
            if (dissolveImages.Length > index)
            {
                if (dissolveImages.Length > 1)
                {
                    dissolveImages[index].fillAmount = 1 - (RemainingTime % dissolveImages.Length) / (defaultTime / dissolveImages.Length);
                }
                else
                {
                    dissolveImages[index].fillAmount = 1 - RemainingTime / (defaultTime / dissolveImages.Length);
                }
            }
        }
    }

    public void AddTime(float time)
    {
        RemainingTime += time;
    }

    public void SetTime(float time)
    {
        RemainingTime = time;
    }

    public void StartTimer()
    {
        if (!UIObject.activeSelf)
        {
            UIObject.SetActive(true);
        }
        ResetTimer();
        IsRunning = true;
    }

    public void ResumeTimer()
    {
        if (IsRunning)
        {
            return;
        }


        if (!UIObject.activeSelf)
        {
            UIObject.SetActive(true);
        }

        IsRunning = true;
    }

    public void PauseTimer()
    {
        if (!IsRunning)
        {
            return;
        }

        IsRunning = false;
    }

    public void StopTimer()
    {
        IsRunning = false;
        OnTimerStop?.Invoke(this);
        UIObject.SetActive(false);
    }

    public void ResetTimer()
    {
        foreach (Image image in dissolveImages)
        {
            image.fillAmount = 0;
        }

        RemainingTime = defaultTime;
    }
}
