using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
            Debug.Log(gameObject.name + "was " + value.ToString());
            Debug.Log("Health: " + City.Citizens.FindAll(c => c.CurrentStatus == Status.Health).Count);
            Debug.Log("Dead: " + City.Citizens.FindAll(c => c.CurrentStatus == Status.Dead).Count);
            Debug.Log("Invisible Infected: " + City.Citizens.FindAll(c => c.CurrentStatus == Status.InvisibleInfected).Count);
            Debug.Log("Infected: " + City.Citizens.FindAll(c => c.CurrentStatus == Status.Infected).Count);
        }
    }
    Status _currentStatus = Status.Health;

    PublicPlace currentActivity;
    int incubationCount = 0, infectedCount = 0;
    Home home;
    NavMeshAgent agent;

    void Awake() {
        City.Instance.WakeupTime += OnWakeupTime;
        City.Instance.HomeTime += OnHomeTime;
        City.Citizens.Add(this);
        agent = GetComponent<NavMeshAgent>();
    }

    public void TryInfect() {
        if (CurrentStatus == Status.InvisibleInfected || CurrentStatus == Status.Infected || CurrentStatus == Status.Recovered) return;

        var result = Random.Range(0f, 1f);
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
            var result = Random.Range(0f, 1f);
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
        while (Vector3.Distance(transform.position, dest.position) > 1) {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, dest.position, Time.deltaTime);
        }
    }
}
