using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicPlace : MonoBehaviour
{
    public static PublicPlace AvailableSpace => publicPlaces.Find(place => place.AvailableCapacity > 0);
    static List<PublicPlace> publicPlaces = new List<PublicPlace>();

    public int AvailableCapacity => capacity - visitors.Count;

    [SerializeField]
    int capacity = 3;

    List<Citizen> visitors = new List<Citizen>();

    void Awake() {
        publicPlaces.Add(this);
        City.Instance.WorkingTime += OnWorkingTime;
        City.Instance.HomeTime += OnHomeTime;
    }

    public bool ReserveTable(Citizen citizen) {
        if (AvailableCapacity <= 0) return false;

        visitors.Add(citizen);
        return true;
    }

    public void LeaveSpace(Citizen citizen) {
        if (!visitors.Contains(citizen)) return;

        visitors.Remove(citizen);
    }

    void OnHomeTime() {
        StopAllCoroutines();
    }

    void OnWorkingTime() {
        StartCoroutine(DoSomeCollaborate());
        Debug.LogError("AAA");
    }

    IEnumerator DoSomeCollaborate() {
        var waitForHour = new WaitForSeconds(60 * 60);
        var sickedVisitors = visitors.FindAll(visitor => visitor.CurrentStatus == Citizen.Status.Infected || visitor.CurrentStatus == Citizen.Status.InvisibleInfected);
        var healthVisitors = visitors.FindAll(visitor => visitor.CurrentStatus != Citizen.Status.Infected && visitor.CurrentStatus != Citizen.Status.InvisibleInfected);

        if (sickedVisitors.Count == 0 || healthVisitors.Count == 0) yield break;

        while (true) {
            foreach(var v in sickedVisitors) {
                var randomVisitor = healthVisitors[Random.Range(0, healthVisitors.Count)];

                randomVisitor.TryInfect();
            }
            yield return waitForHour;
        }
    }
}
