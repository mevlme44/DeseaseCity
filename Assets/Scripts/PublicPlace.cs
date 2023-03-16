using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicPlace : MonoBehaviour
{
    public static List<PublicPlace> PublicPlaces = new List<PublicPlace>();

    public int AvailableCapacity => capacity - visitors.Count;

    [SerializeField]
    int capacity = 3;

    List<Citizen> visitors = new List<Citizen>();

    void Awake() {
        PublicPlaces.Add(this);    
    }

    public bool ReserveTable(Citizen citizen) {
        if (AvailableCapacity <= 0) return false;

        visitors.Add(citizen);
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
