using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField]
    int capacity = 3;
    [SerializeField]
    Citizen prefab;
    [SerializeField]
    Transform spawnPoint;

    void Awake() {
        for (int i = 0; i < capacity; ++i)
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation).SetHome(this);
    }
}
