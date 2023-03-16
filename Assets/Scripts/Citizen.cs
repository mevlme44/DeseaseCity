using System.Collections;
using UnityEngine;

public class Citizen : MonoBehaviour
{
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
        }
    }
    Status _currentStatus = Status.Health;

    PublicPlace currentActivity;
    int incubationCount = 0, infectedCount = 0;
    Vector3 homePos;

    void Awake() {
        City.Instance.WorkingTime += OnWorkingTime;
        City.Instance.HomeTime += OnHomeTime;
    }

    public void TryInfect() {
        if (CurrentStatus == Status.InvisibleInfected || CurrentStatus == Status.Infected || CurrentStatus == Status.Recovered) return;

        var result = Random.Range(0f, 1f);
        if (result > WorldSettings.Active.InfectionProbability) return;

        CurrentStatus = Status.InvisibleInfected;
    }

    void OnHomeTime() {
        StopAllCoroutines();
        if (currentActivity)
            currentActivity.LeaveSpace(this);

        //Translate to home
    }

    void OnWorkingTime() {
        if (CurrentStatus == Status.InvisibleInfected) {
            if (incubationCount < WorldSettings.Active.IncubationPeriod) {
                incubationCount++;
                return;
            }

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
        currentActivity.ReserveTable(this);

        //Translate to work
    }

    void Death() {
        CurrentStatus = Status.Dead;
        Destroy(gameObject);
    }

    IEnumerator DoTreated() {
        infectedCount++;
        var waitForHour = new WaitForSeconds(60 * 60);
        while (true) {
            var result = Random.Range(0f, 1f);
            if (result < WorldSettings.Active.DeathProbability) {
                Death();
                yield break;
            }

            yield return waitForHour;
        }

    }
}
