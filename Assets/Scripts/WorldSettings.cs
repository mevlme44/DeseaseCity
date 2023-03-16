using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings.asset", menuName = "World/Settings Asset")]
public class WorldSettings : ScriptableObject
{
    public static WorldSettings Active {
        get {
            if (!_active) {
                var settings = Resources.LoadAll<WorldSettings>("");
                if (settings.Length == 0) {
                    Debug.LogError("World settings does not presented");
                    return null;
                }
                _active = settings[0];
            }

            return _active;
        }
    }
    static WorldSettings _active;

    [Header("Disease")]
    public float InfectionProbability = 0.01f;
    public float DeathProbability = 0.01f;
    public int TimeToRecover = 3;
    public int IncubationPeriod = 3;

    [Header("City")]
    public int PeopleCount = 10000;
}
