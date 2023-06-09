using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-25)]
public class City : MonoBehaviour
{
    public static City Instance { get; private set; }
    public static List<Citizen> Citizens = new List<Citizen>();

    public DateTime CurrentDate { get; private set; }
    public event Action WakeupTime, WorkingTime, HomeTime;

    public float TimeScale = 1f;

    bool isQuit = false;

    void Awake() {
        Instance = this;
        CurrentDate = DateTime.Now;
        StartCoroutine(DoTickTime());
    }

    [ContextMenu("Add Hour")]
    public void AddHour() {
        CurrentDate = CurrentDate.AddHours(1);
    }

    [ContextMenu("Go to work")]
    public void GoToWork() {
        WakeupTime?.Invoke();
    }

    [ContextMenu("Start work")]
    public void StartWork() {
        WorkingTime?.Invoke();
    }

    [ContextMenu("Go home")]
    public void GoHome() {
        HomeTime?.Invoke();
    }

    IEnumerator DoTickTime() {
        while (!isQuit) {
            yield return new WaitForSeconds(1f / TimeScale);
            CurrentDate = CurrentDate.AddSeconds(1);

                if (CurrentDate.TimeOfDay.Hours == 7 && CurrentDate.TimeOfDay.Minutes == 0 && CurrentDate.TimeOfDay.Seconds == 0) WakeupTime?.Invoke();
                if (CurrentDate.TimeOfDay.Hours == 9 && CurrentDate.TimeOfDay.Minutes == 0 && CurrentDate.TimeOfDay.Seconds == 0) WorkingTime?.Invoke();
                if (CurrentDate.TimeOfDay.Hours == 20 && CurrentDate.TimeOfDay.Minutes == 0 && CurrentDate.TimeOfDay.Seconds == 0) HomeTime?.Invoke();
            
        }
    }
}
