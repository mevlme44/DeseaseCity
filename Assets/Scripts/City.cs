using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public static City Instance { get; private set; }

    public DateTime CurrentDate { get; private set; }
    public event Action WorkingTime, HomeTime;

    [SerializeField]
    DateTime startDate = new DateTime(2000, 0, 0, 0, 0, 0);


    bool isQuit = false;

    void Awake() {
        Instance = this;
        CurrentDate = startDate;
        StartCoroutine(DoTickTime());
    }

    IEnumerator DoTickTime() {
        var waitForSecond = new WaitForSeconds(1f);

        while (!isQuit) {
            yield return waitForSecond;
            CurrentDate.AddSeconds(1);

            if (CurrentDate.DayOfWeek != DayOfWeek.Saturday && CurrentDate.DayOfWeek != DayOfWeek.Sunday) {
                if (CurrentDate.Hour == 8 && CurrentDate.Minute == 0 && CurrentDate.Second == 0) WorkingTime?.Invoke();
                if (CurrentDate.Hour == 20 && CurrentDate.Minute == 0 && CurrentDate.Second == 0) HomeTime?.Invoke();
            }
        }
    }
}
