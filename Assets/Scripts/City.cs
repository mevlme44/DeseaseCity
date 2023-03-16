using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public static City Instance { get; private set; }

    public DateTime CurrentDate { get; private set; }

    [SerializeField]
    DateTime startDate = new DateTime(2000, 0, 0, 0, 0, 0);


    bool isQuit = false;

    void Awake() {
        Instance = this;
        CurrentDate = startDate;
        StartCoroutine(DoTickTime());
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator DoTickTime() {
        var waitForSecond = new WaitForSeconds(1f);

        while (!isQuit) {
            yield return waitForSecond;
            CurrentDate.AddSeconds(1);
        }
    }
}
