using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour
{
    public static event Action ChangeStatus;

    [Serializable]
    public enum Status {
        Health = 0,
        InvisibleInfected = 1,
        Infected = 2,
        Recovered = 3,
        Dead = 4
    }

    public Status CurrentStatus {
        get => _currentStatus;
        
        private set {
            _currentStatus = value;
            Debug.LogError("Status");
            ChangeStatus?.Invoke();
        }
    }
    public Status _currentStatus = Status.Health;

    PublicPlace currentActivity;
    int incubationCount = 0, infectedCount = 0;
    Home home;

    void Awake() {
        City.Instance.WakeupTime += OnWakeupTime;
        City.Instance.HomeTime += OnHomeTime;
        City.Citizens.Add(this);

        if (UnityEngine.Random.Range(0f, 1f) <= WorldSettings.Active.InfectionProbability)
            CurrentStatus = Status.InvisibleInfected;
    }

    public void TryInfect() {
        if (CurrentStatus == Status.InvisibleInfected || CurrentStatus == Status.Infected || CurrentStatus == Status.Recovered) return;

        var result = UnityEngine.Random.Range(0f, 1f);
        if (result > WorldSettings.Active.InfectionProbability) return;

        CurrentStatus = Status.InvisibleInfected;
    }

    public void SetHome(Home h) {
        home = h;
    }

    void OnHomeTime() {
        StopAllCoroutines();
        if (currentActivity)
            currentActivity.LeaveSpace(this);

        StartCoroutine(DoTranslateTo(home.transform));
    }

    void OnWakeupTime() {
        if (CurrentStatus == Status.InvisibleInfected) {
            if (incubationCount < WorldSettings.Active.IncubationPeriod) 
                incubationCount++;
            else 
                CurrentStatus = Status.Infected;
        }

        if (CurrentStatus == Status.Infected) {
            if (infectedCount < WorldSettings.Active.TimeToRecover) {
                StartCoroutine(DoTreated());
                return;
            }

            CurrentStatus = Status.Recovered;
        }

        currentActivity = PublicPlace.AvailableSpace;
        Debug.LogError(currentActivity.name);
        currentActivity.ReserveTable(this);

        StartCoroutine(DoTranslateTo(currentActivity.transform));
    }

    void Death() {
        CurrentStatus = Status.Dead;
        Destroy(gameObject);
    }

    IEnumerator DoTreated() {
        infectedCount++;
        var waitForHour = new WaitForSeconds(60 * 60);
        while (true) {
            var result = UnityEngine.Random.Range(0f, 1f);
            if (result < WorldSettings.Active.DeathProbability) {
                Death();
                yield break;
            }

            yield return waitForHour;
        }

    }

    [ContextMenu("Sick")]
    public void Sick() {
        CurrentStatus = Status.InvisibleInfected;
    }

    IEnumerator DoTranslateTo(Transform dest) {
        var sphere = UnityEngine.Random.insideUnitSphere;
        sphere.y = 0;
        for (float t = 0; t < 10f; t += Time.deltaTime) {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, dest.position + sphere, t);
        }
    }
}
