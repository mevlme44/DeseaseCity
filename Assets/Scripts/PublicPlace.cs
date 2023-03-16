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
}
