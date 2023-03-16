using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{

    public enum Status {
        Health = 0,
        Infected = 1,
        Recovered = 2,
        Dead = 3
    }

    public Status CurrentStatus {
        get => _currentStatus;
        
        private set {
            _currentStatus = value;
        }
    }
    Status _currentStatus = Status.Health;


}
